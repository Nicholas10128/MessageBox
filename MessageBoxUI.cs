using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GCT
{
    namespace UI
    {
        public class MessageBoxUI : MonoBehaviour
        {
            public Text m_Caption;
            public Text m_Content;
            public TMP_Text m_CaptionTMP;
            public TMP_Text m_ContentTMP;

            public GameObject ConfirmOnlyGroup;
            public GameObject YesOrNoGroup;
            public GameObject YesOrNoOrCancelGroup;
            public GameObject NeverDisplayAgainGroup;

            public GameObject m_ConfirmOnlyAutoCloseSeconds;
            public GameObject m_ConfirmOnlyAutoCloseDescription;
            public GameObject m_YesOrNoAutoCloseSeconds;
            public GameObject m_YesOrNoAutoCloseDescription;

            public bool isReady
            {
                get
                {
                    return m_IsReady;
                }
            }

            private StringBuilder m_StringBuilder = new StringBuilder();

            private Text m_ConfirmOnlyAutoCloseSecondsText;
            private Text m_YesOrNoAutoCloseSecondsText;
            private TMP_Text m_ConfirmOnlyAutoCloseSecondsTextTMP;
            private TMP_Text m_YesOrNoAutoCloseSecondsTextTMP;
            private byte m_LastLeftSeconds;

            private MessageBox.MessageInfo m_MessageInfo;
            private Canvas m_Canvas;
            private bool m_IsReady = false;


            void Start()
            {
                if (m_Caption != null)
                {
                    m_Caption.text = string.Empty;
                }
                if (m_Content != null)
                {
                    m_Content.text = string.Empty;
                }
                if (m_CaptionTMP != null)
                {
                    m_CaptionTMP.text = string.Empty;
                }
                if (m_ContentTMP != null)
                {
                    m_ContentTMP.text = string.Empty;
                }
                ConfirmOnlyGroup.SetActive(false);
                YesOrNoGroup.SetActive(false);
                YesOrNoOrCancelGroup.SetActive(false);
                NeverDisplayAgainGroup.SetActive(false);
                m_ConfirmOnlyAutoCloseSecondsText = m_ConfirmOnlyAutoCloseSeconds.GetComponent<Text>();
                m_ConfirmOnlyAutoCloseSecondsTextTMP = m_ConfirmOnlyAutoCloseSeconds.GetComponent<TextMeshProUGUI>();
                m_YesOrNoAutoCloseSecondsText = m_YesOrNoAutoCloseSeconds.GetComponent<Text>();
                m_YesOrNoAutoCloseSecondsTextTMP = m_YesOrNoAutoCloseSeconds.GetComponent<TextMeshProUGUI>();
                m_Canvas = GetComponent<Canvas>();
                m_IsReady = true;
            }

            void Update()
            {
                if (m_MessageInfo != null && !m_MessageInfo.m_Destroying)
                {
                    if (m_MessageInfo.m_AutoClose > 0)
                    {
                        Text autoCloseSecondsText = null;
                        TMP_Text autoCloseSecondsTextTMP = null;
                        switch (m_MessageInfo.m_Type)
                        {
                            case MessageBox.Type.ConfirmOnly:
                                autoCloseSecondsText = m_ConfirmOnlyAutoCloseSecondsText;
                                autoCloseSecondsTextTMP = m_ConfirmOnlyAutoCloseSecondsTextTMP;
                                break;
                            case MessageBox.Type.YesOrNo:
                                autoCloseSecondsText = m_YesOrNoAutoCloseSecondsText;
                                autoCloseSecondsTextTMP = m_YesOrNoAutoCloseSecondsTextTMP;
                                break;
                            default:
                                Debug.LogError("Fatal error! m_MessageInfo.m_Type = " + m_MessageInfo.m_Type);
                                return;
                        }
                        float elapsed = Time.time - m_MessageInfo.m_BeginTime;
                        byte leftSeconds = (byte)Mathf.CeilToInt(m_MessageInfo.m_AutoClose - elapsed);
                        if (m_LastLeftSeconds != leftSeconds)
                        {
                            m_StringBuilder.Append(string.Format(m_MessageInfo.m_LeftSecondsFormatText, m_LastLeftSeconds = leftSeconds));
                            if (autoCloseSecondsText != null)
                            {
                                autoCloseSecondsText.text = m_StringBuilder.ToString();
                            }
                            if (autoCloseSecondsTextTMP != null)
                            {
                                autoCloseSecondsTextTMP.text = m_StringBuilder.ToString();
                            }
                            m_StringBuilder.Clear();
                        }
                    }
                }
            }

            public void SetContentAlignment(TextAnchor alignment)
            {
                m_Content.alignment = alignment;
            }

            public void OnOpen(MessageBox.MessageInfo mi)
            {
                m_MessageInfo = mi;
                if (m_Caption != null)
                {
                    m_Caption.text = m_MessageInfo.m_Caption;
                }
                if (m_Content != null)
                {
                    m_Content.text = m_MessageInfo.m_Content;
                    m_Content.alignment = m_MessageInfo.m_ContentAlignment;
                }
                if (m_CaptionTMP != null)
                {
                    m_CaptionTMP.text = m_MessageInfo.m_Caption;
                }
                if (m_ContentTMP != null)
                {
                    m_ContentTMP.text = m_MessageInfo.m_Content;
                    m_ContentTMP.alignment = m_MessageInfo.m_ContentAlignmentTMP;
                }

                switch (m_MessageInfo.m_Type)
                {
                    case MessageBox.Type.ConfirmOnly:
                        ConfirmOnlyGroup.SetActive(true);
                        YesOrNoGroup.SetActive(false);
                        YesOrNoOrCancelGroup.SetActive(false);
                        NeverDisplayAgainGroup.SetActive(false);
                        if (m_MessageInfo.m_AutoClose == 0)
                        {
                            m_ConfirmOnlyAutoCloseSeconds.SetActive(false);
                            m_ConfirmOnlyAutoCloseDescription.SetActive(false);
                        }
                        else
                        {
                            m_ConfirmOnlyAutoCloseSeconds.SetActive(true);
                            m_ConfirmOnlyAutoCloseDescription.SetActive(true);
                            m_StringBuilder.Append(string.Format(m_MessageInfo.m_LeftSecondsFormatText, m_LastLeftSeconds = m_MessageInfo.m_AutoClose));
                            if (m_ConfirmOnlyAutoCloseSecondsText != null)
                            {
                                m_ConfirmOnlyAutoCloseSecondsText.text = m_StringBuilder.ToString();
                            }
                            if (m_ConfirmOnlyAutoCloseSecondsTextTMP != null)
                            {
                                m_ConfirmOnlyAutoCloseSecondsTextTMP.text = m_StringBuilder.ToString();
                            }
                            m_StringBuilder.Clear();
                            m_MessageInfo.m_BeginTime = Time.time;
                        }
                        break;
                    case MessageBox.Type.YesOrNo:
                        ConfirmOnlyGroup.SetActive(false);
                        YesOrNoGroup.SetActive(true);
                        YesOrNoOrCancelGroup.SetActive(false);
                        NeverDisplayAgainGroup.SetActive(false);
                        if (m_MessageInfo.m_AutoClose == 0)
                        {
                            m_YesOrNoAutoCloseSeconds.SetActive(false);
                            m_YesOrNoAutoCloseDescription.SetActive(false);
                        }
                        else
                        {
                            m_YesOrNoAutoCloseSeconds.SetActive(true);
                            m_YesOrNoAutoCloseDescription.SetActive(true);
                            m_StringBuilder.Append(m_LastLeftSeconds = m_MessageInfo.m_AutoClose);
                            if (m_YesOrNoAutoCloseSecondsText != null)
                            {
                                m_YesOrNoAutoCloseSecondsText.text = m_StringBuilder.ToString();
                            }
                            if (m_YesOrNoAutoCloseSecondsTextTMP != null)
                            {
                                m_YesOrNoAutoCloseSecondsTextTMP.text = m_StringBuilder.ToString();
                            }
                            m_StringBuilder.Clear();
                            m_MessageInfo.m_BeginTime = Time.time;
                        }
                        break;
                    case MessageBox.Type.YesOrNoOrCancel:
                        ConfirmOnlyGroup.SetActive(false);
                        YesOrNoGroup.SetActive(false);
                        YesOrNoOrCancelGroup.SetActive(true);
                        NeverDisplayAgainGroup.SetActive(false);
                        break;
                    case MessageBox.Type.NeverDisplayAgain:
                        ConfirmOnlyGroup.SetActive(false);
                        YesOrNoGroup.SetActive(false);
                        YesOrNoOrCancelGroup.SetActive(false);
                        NeverDisplayAgainGroup.SetActive(true);
                        break;
                }
            }

            public void OnConfirmButtonClick()
            {
                m_MessageInfo.m_Callback?.Invoke(MessageBox.ButtonID.Confirm, m_MessageInfo.m_Parameter);
                OnClose();
            }

            public void OnCancelButtonClick()
            {
                m_MessageInfo.m_Callback?.Invoke(MessageBox.ButtonID.Cancel, m_MessageInfo.m_Parameter);
                OnClose();
            }

            public void OnCloseButtonClick()
            {
                m_MessageInfo.m_Callback?.Invoke(MessageBox.ButtonID.Close, m_MessageInfo.m_Parameter);
                OnClose();
            }

            private void OnClose()
            {
                m_MessageInfo.m_Destroying = true;
                m_Canvas.enabled = false;
            }
        }
    }
}
