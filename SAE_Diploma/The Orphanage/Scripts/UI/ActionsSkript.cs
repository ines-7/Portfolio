using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Orphanage.Utility;
using UnityEngine.InputSystem.UI;

namespace Orphanage
{
    namespace UI
    {
        public class ActionsSkript : MonoBehaviour
        {
            #region Variablen
            [Header ("Refrence")]
            [SerializeField]
            private InputActionReference m_refrence;
            [SerializeField]
            private TextMeshProUGUI m_buttonText;
            [SerializeField]
            private GameObject m_waitText;
            [SerializeField]
            private GameObject m_repeatText;

            [Header ("Settings")]
            [SerializeField]
            private int m_bindingNumber;

            private string m_device = string.Empty;
            private InputActionRebindingExtensions.RebindingOperation m_rebind;
            #endregion

            private void Awake()
            {
                m_device = Device();

                if(m_device.ToLower() == "gamepad")
                {
                    ChangeGamepadBindingNumber();
                }
            }

            /// <summary>
            /// Setzt die Overrides mithilfe der geholten PlayerPrefs.
            /// Außerdem wir nach original Steuerung kontrolliert und setzt dementsprechend den Reset-Button aktiv(interactable = true)
            /// Die Namen in den Optionen werden draufhin festgelegt
            /// </summary>
            private void Start()
            {

                if (m_refrence.action.name.ToLower() == "basicmovement")
                    m_refrence.action.ApplyBindingOverride(m_bindingNumber, PlayerPrefs.GetString(m_refrence.action.bindings[m_bindingNumber].name,
                                                                                                    m_refrence.action.bindings[m_bindingNumber].effectivePath));
                else
                    m_refrence.action.ApplyBindingOverride(m_bindingNumber, PlayerPrefs.GetString(m_refrence.action.name,
                                                                                                    m_refrence.action.bindings[m_bindingNumber].effectivePath));

                if (m_device.ToLower() == "keyboard")
                {
                    if (InputControlPath.ToHumanReadableString(m_refrence.action.bindings[m_bindingNumber].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice).ToLower()
                        != KeyboardOrigin().ToLower())
                        OptionsPanel.Instance.ActivateControlReset();
                }
                else
                {
                    if (InputControlPath.ToHumanReadableString(m_refrence.action.bindings[m_bindingNumber].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice).ToLower()
                        != GamepadOrigin().ToLower())
                        OptionsPanel.Instance.ActivateControlReset();
                }

                ChangeName();
            }

            #region Changings
            /// <summary>
            /// Setzt den Text des Buttons neu
            /// </summary>
            private void ChangeName()
            {
                string text = InputControlPath.ToHumanReadableString(m_refrence.action.bindings[m_bindingNumber].effectivePath,
                                                                        InputControlPath.HumanReadableStringOptions.OmitDevice);

                m_buttonText.text = text;
            }

            /// <summary>
            /// Methode die das Rebinding startet
            /// </summary>
            public void ChangeBinding()
            {
                m_buttonText.gameObject.SetActive(false);
                m_waitText.SetActive(true);

                StartRebind();

                OptionsPanel.Instance.ActivateControlReset();
            }

            /// <summary>
            /// Ändert die Binding Nummer, falls Device ein Gamepad ist
            /// </summary>
            private void ChangeGamepadBindingNumber()
            {
                if (m_refrence.action.name.ToLower() == "basicmovement")
                {
                    m_bindingNumber = m_bindingNumber + 4;
                }
                else
                {
                    m_bindingNumber = m_bindingNumber + 1;
                }
            }
            #endregion

            #region Rebinding
            /// <summary>
            /// Die Bindings der Action werden versucht neu gesetzt zu werden
            /// </summary>
            private void StartRebind()
            {
                m_rebind?.Cancel();


                m_refrence.action.Disable();
                if (m_device.ToLower() == "keyboard")
                {
                    //Rebinding für das Keyboard
                    m_rebind = m_refrence.action.PerformInteractiveRebinding(m_bindingNumber)
                        .WithControlsExcluding("Mouse")
                        .WithCancelingThrough("<Keyboard>/escape")
                        .OnMatchWaitForAnother(0.2f);
                }
                else
                {
                    //Rebinding für das Gamepad
                    m_rebind = m_refrence.action.PerformInteractiveRebinding(m_bindingNumber)
                        .OnMatchWaitForAnother(0.2f);
                }

                m_rebind.Start()
                        .OnCancel(s =>
                        {
                            m_refrence.action.Enable();
                            Debug.Log("Cancel");
                            CleanUp();
                        })
                        .OnComplete(s =>
                        {
                            m_refrence.action.Enable();
                            Debug.Log("Finish");

                            if (DuplicateBindings())
                            {
                                m_refrence.action.RemoveBindingOverride(m_bindingNumber);
                                m_repeatText.SetActive(true);
                                CleanUp();
                                ChangeBinding();
                                return;
                            }

                            SaveBinding();
                            CleanUp();

                            if (m_repeatText.activeInHierarchy)
                                m_repeatText.SetActive(false);

                            GetSettings.Instance.Changes = true;
                        });
            }

            /// <summary>
            /// Fügt bereits verwendete Keys zu den Excludings hinzu
            /// </summary>
            private void ExistingBindings()
            {
                InputActionMap map = m_refrence.asset.FindActionMap("movement");
                InputAction action;

                string binding = InputControlPath.ToHumanReadableString(m_refrence.action.bindings[m_bindingNumber].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                string text = string.Empty;

                for(int i = 0; i < map.actions.Count; i++)
                {
                    action = map.actions[i];
                    for (int j = 0; j < action.bindings.Count; j++)
                    {
                        text = InputControlPath.ToHumanReadableString(action.bindings[j].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                        if (text != "2DVector")
                        {
                            if(action.bindings[j].name != m_refrence.action.bindings[m_bindingNumber].name)
                                m_rebind.WithControlsExcluding(action.bindings[j].effectivePath);
                        }
                    }
                }
            }

            /// <summary>
            /// Kontrolliert, ob es ein Binding doppelt gibt
            /// </summary>
            /// <returns></returns>
            private bool DuplicateBindings()
            {
                InputBinding inputBinding = m_refrence.action.bindings[m_bindingNumber];

                foreach(InputBinding binding in m_refrence.action.actionMap.bindings)
                {
                    if(binding.action == inputBinding.action)
                    {
                        continue;
                    }
                    if(binding.effectivePath == inputBinding.effectivePath)
                    {
                        Debug.Log($"Duplikat: {binding.effectivePath}");
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Setzt den neuen Namen und löst das Rebinding auf
            /// </summary>
            private void CleanUp()
            {
                m_rebind?.Dispose();
                m_rebind = null;

                ChangeName();

                m_buttonText.gameObject.SetActive(true);
                m_waitText.SetActive(false);
            }

            /// <summary>
            /// Das neue Binding wird in einem PlayerPrefs gespeichert
            /// </summary>
            private void SaveBinding()
            {
                string device = string.Empty;
                string key = string.Empty;

                string text = m_refrence.action.bindings[m_bindingNumber].name;

                m_refrence.action.GetBindingDisplayString(m_bindingNumber, out device, out key);
                Debug.Log($"Device: {device}/ Key: {key}");

                if (m_refrence.action.name.ToLower() == "basicmovement")
                    PlayerPrefs.SetString(m_refrence.action.bindings[m_bindingNumber].name, $"<{device}>/{key}");
                else
                    PlayerPrefs.SetString(m_refrence.action.name, $"<{device}>/{key}");

                PlayerPrefs.Save();
            }
            #endregion

            #region Reset
            /// <summary>
            /// Setzt die neu gesetzten Bindings auf den Ursprung zurück
            /// </summary>
            public void Reset()
            {
                m_refrence.action.RemoveAllBindingOverrides();
                SaveBinding();
                ChangeName();
            }
            #endregion

            #region Strings
            /// <summary>
            /// Holt den Device Namen
            /// </summary>
            /// <returns>Keyboard oder Gamepad</returns>
            private string Device()
            {
                string device = string.Empty;
                string key = string.Empty;

                m_refrence.action.GetBindingDisplayString(m_bindingNumber, out device, out key);

                return device;
            }

            /// <summary>
            /// Holt die ursprüngliche Taste für die Action
            /// </summary>
            /// <returns>Keyboard Taste</returns>
            private string KeyboardOrigin()
            {
                switch(m_refrence.action.name.ToLower())
                {
                    case "basicmovement":
                        {
                            switch(m_bindingNumber)
                            {
                                case 1:
                                    return "w";
                                case 2:
                                    return "s";
                                case 3:
                                    return "a";
                                case 4:
                                    return "d";
                            }
                            break;
                        }
                    case "jump":
                        {
                            return "space";
                        }
                    case "crouch":
                        { 
                            return "c";
                        }
                    case "sprint":
                        {
                            return "leftShift";
                        }
                    case "action":
                        {
                            return "e";
                        }
                }

                return "";
            }

            /// <summary>
            /// Holt die ursprüngliche Taste für die Action
            /// </summary>
            /// <returns>Gampad Taste</returns>
            private string GamepadOrigin()
            {
                switch (m_refrence.action.name.ToLower())
                {
                    case "basicmovement":
                        {
                            switch (m_bindingNumber)
                            {
                                case 5:
                                    return "leftStick/up";
                                case 6:
                                    return "leftStick/down";
                                case 7:
                                    return "leftStick/left";
                                case 8:
                                    return "leftStick/right";
                            }
                            break;
                        }
                    case "jump":
                        {
                            return "buttonSouth";
                        }
                    case "crouch":
                        {
                            return "buttonEast";
                        }
                    case "sprint":
                        {
                            return "leftShoulder";
                        }
                    case "action":
                        {
                            return "rightShoulder";
                        }
                }

                return "";
            }
            #endregion
        }
    }
}