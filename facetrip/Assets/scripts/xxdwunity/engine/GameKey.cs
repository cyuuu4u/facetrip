using UnityEngine;
using System;
using xxdwunity.util;

namespace xxdwunity.engine
{
    public class GameKey
    {
        public enum EKey
        {
            // 按键
            K_NULL, K_Escape, K_Return, K_Backspace, K_Space,                               
            K_UpArrow, K_DownArrow, K_LeftArrow, K_RightArrow,
            K_W, K_S, K_A, K_R, 
            // 修改键
            K_Menu, K_LeftControl, K_RightControl, K_LeftAlt, K_RightAlt, 
            K_LeftShift, K_RightShift, K_LeftApple, K_RightApple
        };

        private EKey modifier   = EKey.K_NULL;

        private EKey[] keys     = { EKey.K_Escape, EKey.K_Return, EKey.K_Backspace, EKey.K_Space,                               
                                EKey.K_UpArrow, EKey.K_DownArrow, EKey.K_LeftArrow, EKey.K_RightArrow,
                                EKey.K_W, EKey.K_S, EKey.K_A, EKey.K_R };
        private EKey[] modifiers = { EKey.K_Menu, EKey.K_LeftControl, EKey.K_RightControl, EKey.K_LeftAlt, EKey.K_RightAlt, 
                                EKey.K_LeftShift, EKey.K_RightShift, EKey.K_LeftApple, EKey.K_RightApple };

        private KeyCode[] codes = { KeyCode.Escape, KeyCode.Return, KeyCode.Backspace, KeyCode.Space,                               
                              KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
                              KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.R};
        private KeyCode[] modifierCodes = {KeyCode.Menu, KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftAlt, KeyCode.RightAlt, 
                              KeyCode.LeftShift, KeyCode.RightShift, KeyCode.LeftApple, KeyCode.RightApple};

        public EKey GetModifier()
        {
            return this.modifier;
        }

        public EKey GetKeyDown()
        {
            this.modifier = EKey.K_NULL;

            for (int i = 0; i < codes.Length; i++)
            {
                if (Input.GetKeyDown(codes[i]) || Input.GetKey(codes[i]))
                {
                    this.modifier = GetPressingModifierKey();
                    return keys[i];
                }
            }

            return EKey.K_NULL;
        }

        public EKey GetKeyUp()
        {
            for (int i = 0; i < codes.Length; i++)
            {
                if (Input.GetKeyUp(codes[i]))
                    return keys[i];
            }
            return EKey.K_NULL;
        }

        public EKey GetPressingModifierKey()
        {
            for (int j = 0; j < modifierCodes.Length; j++)
            {
                if (Input.GetKey(modifierCodes[j]))
                {
                    return modifiers[j];
                }
            }

            return EKey.K_NULL;
        }

        public String GetKeyString(EKey k)
        {
            string s = "";
            switch (k)
            {
            case EKey.K_NULL:
	            s = "NULL";
	            break;
            case EKey.K_Escape:
	            s = "Escape";
	            break;
            case EKey.K_Return:
	            s = "Return";
	            break;
            case EKey.K_Backspace:
	            s = "Backspace";
	            break;
            case EKey.K_Space:
	            s = "Space";
	            break;
            case EKey.K_UpArrow:
	            s = "UpArrow";
	            break;
            case EKey.K_DownArrow:
	            s = "DownArrow";
	            break;
            case EKey.K_LeftArrow:
	            s = "LeftArrow";
	            break;
            case EKey.K_RightArrow:
	            s = "RightArrow";
	            break;
            case EKey.K_W:
	            s = "W";
	            break;
            case EKey.K_S:
	            s = "S";
	            break;
            case EKey.K_A:
	            s = "A";
	            break;
            case EKey.K_R:
	            s = "R";
	            break;
            case EKey.K_Menu:
	            s = "Menu";
	            break;
            case EKey.K_LeftControl:
	            s = "LeftControl";
	            break;
            case EKey.K_RightControl:
	            s = "RightControl";
	            break;
            case EKey.K_LeftAlt:
	            s = "LeftAlt";
	            break;
            case EKey.K_RightAlt:
	            s = "RightAlt";
	            break;
            case EKey.K_LeftShift:
	            s = "LeftShift";
	            break;
            case EKey.K_RightShift:
	            s = "RightShift";
	            break;
            case EKey.K_LeftApple:
	            s = "LeftApple";
	            break;
            case EKey.K_RightApple:
	            s = "RightApple";
	            break;
            }
            return s;
        }

        public static bool IsControlKey(EKey key)
        {
            return key == EKey.K_LeftControl || key == EKey.K_RightControl;
        }

        public static bool IsAltKey(EKey key)
        {
            return key == EKey.K_LeftAlt || key == EKey.K_RightAlt;
        }

        public static bool IsShiftKey(EKey key)
        {
            return key == EKey.K_LeftShift || key == EKey.K_RightShift;
        }
    } // class
}
