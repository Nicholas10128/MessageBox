using System.Collections.Generic;
using UnityEngine;

namespace GCT
{
    namespace UI
    {
        public class MessageBox
        {
            public enum Type
            {
                ConfirmOnly,
                YesOrNo,
                YesOrNoOrCancel,
                NeverDisplayAgain
            }

            public enum ButtonID
            {
                Confirm,
                Cancel,
                Close
            }

            public delegate void Callback(ButtonID bid, object parameter);

            public class MessageInfo
            {
                public Type m_Type;
                public string m_Caption;
                public string m_Content;
                public TextAnchor m_ContentAlignment;
                public Callback m_Callback;
                public object m_Parameter;
                public byte m_AutoClose;

                public bool m_Destroying;
                public float m_BeginTime;
            }

            private static Queue<MessageInfo> m_MessageQueue = new Queue<MessageInfo>();
            private static MessageInfo m_CurrentMessage = null;
            private static Canvas m_MessageBoxUI = null;

            public static void Initialize()
            {
                GameObject go = GameObject.Find("MessageBox");
                m_MessageBoxUI = go.GetComponent<Canvas>();
            }

            public static void Show(string caption, string content, Type type, TextAnchor contentAlignment, Callback callback, object parameter)
            {
                MessageInfo mi = new MessageInfo();
                mi.m_Caption = caption;
                mi.m_Content = content;
                mi.m_Type = type;
                mi.m_ContentAlignment = contentAlignment;
                mi.m_Callback = callback;
                mi.m_Parameter = parameter;
                mi.m_AutoClose = 0;
                mi.m_Destroying = false;
                mi.m_BeginTime = 0;
                m_MessageQueue.Enqueue(mi);
            }

            public static void Show(string caption, string content, TextAnchor contentAlignment, Callback callback, object parameter, byte autoCloseSeconds)
            {
                MessageInfo mi = new MessageInfo();
                mi.m_Caption = caption;
                mi.m_Content = content;
                mi.m_Type = Type.ConfirmOnly;
                mi.m_ContentAlignment = contentAlignment;
                mi.m_Callback = callback;
                mi.m_Parameter = parameter;
                mi.m_AutoClose = autoCloseSeconds;
                mi.m_Destroying = false;
                mi.m_BeginTime = 0;
                m_MessageQueue.Enqueue(mi);
            }

            public static void Tick()
            {
                if (m_CurrentMessage != null && !m_CurrentMessage.m_Destroying)
                {
                    if (m_CurrentMessage.m_AutoClose > 0)
                    {
                        float elapsed = Time.time - m_CurrentMessage.m_BeginTime;
                        if (elapsed >= m_CurrentMessage.m_AutoClose)
                        {
                            m_MessageBoxUI.SendMessage("OnConfirmButtonClick");
                        }
                    }
                    return;
                }

                if (m_MessageQueue.Count > 0)
                {
                    m_CurrentMessage = m_MessageQueue.Dequeue();
                    m_MessageBoxUI.enabled = true;
                    m_MessageBoxUI.SendMessage("OnOpen", m_CurrentMessage);
                }
            }
        }
    }
}
