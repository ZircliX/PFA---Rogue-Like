using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.MUIP
{
    public class WindowManager : MonoBehaviour
    {
        public List<WindowItem> windows = new List<WindowItem>();

        public int currentWindowIndex = 0;
        private int currentButtonIndex = 0;
        private int newWindowIndex;
        public bool cullWindows = true;
        public bool initializeButtons = true;
        bool isInitialized = false;

        [System.Serializable] public class WindowChangeEvent : UnityEvent<int> { }
        public WindowChangeEvent onWindowChange;

        private GameObject currentWindow;
        private GameObject nextWindow;
        private GameObject currentButton;
        private GameObject nextButton;
        private Animator currentWindowAnimator;
        private Animator nextWindowAnimator;
        private Animator currentButtonAnimator;
        private Animator nextButtonAnimator;

        string windowFadeIn = "In";
        string windowFadeOut = "Out";
        string buttonFadeIn = "Hover to Pressed";
        string buttonFadeOut = "Pressed to Normal";
        float cachedStateLength;
        public bool altMode;

        [System.Serializable]
        public class WindowItem
        {
            public string windowName = "My Window";
            public GameObject windowObject;
            public GameObject buttonObject;
            public GameObject firstSelected;
        }

        void Awake()
        {
            if (windows.Count == 0)
                return;

            InitializeWindows();
        }

        void OnEnable()
        {
            if (isInitialized == true && nextWindowAnimator == null)
            {
                currentWindowAnimator?.Play(windowFadeIn);
                currentButtonAnimator?.Play(buttonFadeIn);
            }

            else if (isInitialized == true && nextWindowAnimator != null)
            {
                nextWindowAnimator.Play(windowFadeIn);
                nextButtonAnimator?.Play(buttonFadeIn);
            }
        }

        public void InitializeWindows()
        {
            if (windows[currentWindowIndex].firstSelected != null)
                EventSystem.current.firstSelectedGameObject = windows[currentWindowIndex].firstSelected;

            if (windows[currentWindowIndex].buttonObject != null)
            {
                currentButton = windows[currentWindowIndex].buttonObject;
                if (currentButton.TryGetComponent(out Animator buttonAnim))
                {
                    currentButtonAnimator = buttonAnim;
                    currentButtonAnimator.Play(buttonFadeIn);
                }
            }

            currentWindow = windows[currentWindowIndex].windowObject;
            if (currentWindow.TryGetComponent(out Animator windowAnim))
            {
                currentWindowAnimator = windowAnim;
                currentWindowAnimator.Play(windowFadeIn);
            }

            onWindowChange.Invoke(currentWindowIndex);

            cachedStateLength = altMode ? 0.3f : MUIPInternalTools.GetAnimatorClipLength(currentWindowAnimator, MUIPInternalTools.windowManagerStateName);
            isInitialized = true;

            for (int i = 0; i < windows.Count; i++)
            {
                if (i != currentWindowIndex && cullWindows == true)
                    windows[i].windowObject.SetActive(false);

                if (windows[i].buttonObject != null && initializeButtons == true)
                {
                    string tempName = windows[i].windowName;
                    if (windows[i].buttonObject.TryGetComponent(out ButtonManager tempButton))
                    {
                        tempButton.onClick.RemoveAllListeners();
                        tempButton.onClick.AddListener(() => OpenPanel(tempName));
                    }
                }
            }
        }

        public void OpenFirstTab()
        {
            if (currentWindowIndex != 0)
            {
                currentWindow = windows[currentWindowIndex].windowObject;
                if (currentWindow.TryGetComponent(out currentWindowAnimator))
                    currentWindowAnimator.Play(windowFadeOut);

                if (windows[currentWindowIndex].buttonObject != null)
                {
                    currentButton = windows[currentWindowIndex].buttonObject;
                    if (currentButton.TryGetComponent(out currentButtonAnimator))
                        currentButtonAnimator.Play(buttonFadeOut);
                }

                currentWindowIndex = 0;
                currentButtonIndex = 0;

                currentWindow = windows[currentWindowIndex].windowObject;
                if (currentWindow.TryGetComponent(out currentWindowAnimator))
                    currentWindowAnimator.Play(windowFadeIn);

                if (windows[currentWindowIndex].firstSelected != null)
                    EventSystem.current.firstSelectedGameObject = windows[currentWindowIndex].firstSelected;

                if (windows[currentButtonIndex].buttonObject != null)
                {
                    currentButton = windows[currentButtonIndex].buttonObject;
                    if (currentButton.TryGetComponent(out currentButtonAnimator))
                        currentButtonAnimator.Play(buttonFadeIn);
                }

                onWindowChange.Invoke(currentWindowIndex);
            }
            else
            {
                currentWindow = windows[currentWindowIndex].windowObject;
                if (currentWindow.TryGetComponent(out currentWindowAnimator))
                    currentWindowAnimator.Play(windowFadeIn);

                if (windows[currentWindowIndex].firstSelected != null)
                    EventSystem.current.firstSelectedGameObject = windows[currentWindowIndex].firstSelected;

                if (windows[currentButtonIndex].buttonObject != null)
                {
                    currentButton = windows[currentButtonIndex].buttonObject;
                    if (currentButton.TryGetComponent(out currentButtonAnimator))
                        currentButtonAnimator.Play(buttonFadeIn);
                }
            }
        }

        public void OpenWindow(string newWindow)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowName == newWindow)
                {
                    newWindowIndex = i;
                    break;
                }
            }

            if (newWindowIndex != currentWindowIndex)
            {
                if (cullWindows == true)
                    StopCoroutine("DisablePreviousWindow");

                currentWindow = windows[currentWindowIndex].windowObject;
                currentButton = windows[currentWindowIndex].buttonObject;
                currentWindowIndex = newWindowIndex;

                nextWindow = windows[currentWindowIndex].windowObject;
                nextWindow.SetActive(true);

                currentWindow.TryGetComponent(out currentWindowAnimator);
                nextWindow.TryGetComponent(out nextWindowAnimator);

                currentWindowAnimator?.Play(windowFadeOut);
                nextWindowAnimator?.Play(windowFadeIn);

                if (cullWindows == true)
                    StartCoroutine("DisablePreviousWindow");

                currentButtonIndex = newWindowIndex;

                if (windows[currentWindowIndex].firstSelected != null)
                    EventSystem.current.firstSelectedGameObject = windows[currentWindowIndex].firstSelected;

                if (windows[currentButtonIndex].buttonObject != null)
                {
                    nextButton = windows[currentButtonIndex].buttonObject;
                    currentButton.TryGetComponent(out currentButtonAnimator);
                    nextButton.TryGetComponent(out nextButtonAnimator);
                    currentButtonAnimator?.Play(buttonFadeOut);
                    nextButtonAnimator?.Play(buttonFadeIn);
                }

                onWindowChange.Invoke(currentWindowIndex);
            }
        }

        public void OpenPanel(string newPanel) => OpenWindow(newPanel);

        public void OpenWindowByIndex(int windowIndex) => OpenWindow(windows[windowIndex].windowName);

        public void NextWindow()
        {
            if (currentWindowIndex <= windows.Count - 2)
            {
                if (cullWindows == true)
                    StopCoroutine("DisablePreviousWindow");

                currentWindow = windows[currentWindowIndex].windowObject;
                currentWindow.SetActive(true);

                if (windows[currentButtonIndex].buttonObject != null)
                {
                    currentButton = windows[currentButtonIndex].buttonObject;
                    nextButton = windows[currentButtonIndex + 1].buttonObject;
                    currentButton.TryGetComponent(out currentButtonAnimator);
                    currentButtonAnimator?.Play(buttonFadeOut);
                }

                currentWindow.TryGetComponent(out currentWindowAnimator);
                currentWindowAnimator?.Play(windowFadeOut);

                currentWindowIndex++;
                currentButtonIndex++;

                nextWindow = windows[currentWindowIndex].windowObject;
                nextWindow.SetActive(true);
                nextWindow.TryGetComponent(out nextWindowAnimator);
                nextWindowAnimator?.Play(windowFadeIn);

                if (cullWindows == true)
                    StartCoroutine("DisablePreviousWindow");

                if (windows[currentWindowIndex].firstSelected != null)
                    EventSystem.current.firstSelectedGameObject = windows[currentWindowIndex].firstSelected;

                nextButton?.TryGetComponent(out nextButtonAnimator);
                nextButtonAnimator?.Play(buttonFadeIn);

                onWindowChange.Invoke(currentWindowIndex);
            }
        }

        public void PrevWindow()
        {
            if (currentWindowIndex >= 1)
            {
                if (cullWindows == true)
                    StopCoroutine("DisablePreviousWindow");

                currentWindow = windows[currentWindowIndex].windowObject;
                currentWindow.SetActive(true);

                if (windows[currentButtonIndex].buttonObject != null)
                {
                    currentButton = windows[currentButtonIndex].buttonObject;
                    nextButton = windows[currentButtonIndex - 1].buttonObject;
                    currentButton.TryGetComponent(out currentButtonAnimator);
                    currentButtonAnimator?.Play(buttonFadeOut);
                }

                currentWindow.TryGetComponent(out currentWindowAnimator);
                currentWindowAnimator?.Play(windowFadeOut);

                currentWindowIndex--;
                currentButtonIndex--;

                nextWindow = windows[currentWindowIndex].windowObject;
                nextWindow.SetActive(true);
                nextWindow.TryGetComponent(out nextWindowAnimator);
                nextWindowAnimator?.Play(windowFadeIn);

                if (cullWindows == true)
                    StartCoroutine("DisablePreviousWindow");

                if (windows[currentWindowIndex].firstSelected != null)
                    EventSystem.current.firstSelectedGameObject = windows[currentWindowIndex].firstSelected;

                nextButton?.TryGetComponent(out nextButtonAnimator);
                nextButtonAnimator?.Play(buttonFadeIn);

                onWindowChange.Invoke(currentWindowIndex);
            }
        }

        public void ShowCurrentWindow() => (nextWindowAnimator ?? currentWindowAnimator)?.Play(windowFadeIn);
        public void HideCurrentWindow() => (nextWindowAnimator ?? currentWindowAnimator)?.Play(windowFadeOut);
        public void ShowCurrentButton() => (nextButtonAnimator ?? currentButtonAnimator)?.Play(buttonFadeIn);
        public void HideCurrentButton() => (nextButtonAnimator ?? currentButtonAnimator)?.Play(buttonFadeOut);

        public void AddNewItem()
        {
            WindowItem window = new WindowItem();

            if (windows.Count != 0 && windows[^1].windowObject != null)
            {
                int tempIndex = windows.Count - 1;

                GameObject tempWindow = windows[tempIndex].windowObject.transform.parent.GetChild(tempIndex).gameObject;
                GameObject newWindow = Instantiate(tempWindow);
                newWindow.transform.SetParent(windows[tempIndex].windowObject.transform.parent, false);
                newWindow.name = "New Window " + tempIndex;

                window.windowName = newWindow.name;
                window.windowObject = newWindow;

                if (windows[tempIndex].buttonObject != null)
                {
                    GameObject tempButton = windows[tempIndex].buttonObject.transform.parent.GetChild(tempIndex).gameObject;
                    GameObject newButton = Instantiate(tempButton);
                    newButton.transform.SetParent(windows[tempIndex].buttonObject.transform.parent, false);
                    newButton.name = newWindow.name;

                    window.buttonObject = newButton;
                }
            }

            windows.Add(window);
        }

        IEnumerator DisablePreviousWindow()
        {
            yield return new WaitForSecondsRealtime(cachedStateLength);
            for (int i = 0; i < windows.Count; i++)
            {
                if (i != currentWindowIndex)
                    windows[i].windowObject.SetActive(false);
            }
        }
    }
}