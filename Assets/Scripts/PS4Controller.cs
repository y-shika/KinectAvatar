using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PS4Controller : MonoBehaviour {

	public static bool isConnect()
	{
		var controllerName = Input.GetJoystickNames();

		// macだと"Sony Interactive Entertainment Wireless Controller"、Windowsだと"Wireless Controller"と認識されるよう。
		if(controllerName.Length > 0 && (controllerName.Contains("Sony Interactive Entertainment Wireless Controller") || controllerName.Contains("Wireless Controller"))) {
			return true;
		}
		return false;
	}

    // 仕様では入力は16まである想定のため、Facialボタンと記号ボタンの16通りで行う。
    // 更新 : PSボタンがBluetooth関連で使えないことから、FacialPattern4のTouchパッドは、CameraSwitcherなどに回す
    public static bool FacialButtonDown(int i)
    {
        if(i >= 0 && i < 4)
        {
            return Input.GetButton("FacialPattern1") && SymbolButtonDown(i);
        }
        else if(i >= 4 && i < 8)
        {
            return Input.GetButton("FacialPattern2") && SymbolButtonDown(i - 4);
        }
        else if(i >= 8 && i < 12)
        {
            return Input.GetButton("FacialPattern3") && SymbolButtonDown(i - 8);
        }
        // else if(i >= 12 && i < 16)
        // {
        //     return Input.GetButton("FacialPattern4") && SymbolButtonDown(i - 12);
        // }

        return false;
    }

    public static bool FacialButtonUp(int i)
    {
        if(i >= 0 && i < 4)
        {
            return Input.GetButtonUp("FacialPattern1") || SymbolButtonUp(i);
        }
        else if(i >= 4 && i < 8)
        {
            return Input.GetButtonUp("FacialPattern2") || SymbolButtonUp(i - 4);
        }
        else if(i >= 8 && i < 12)
        {
            return Input.GetButtonUp("FacialPattern3") || SymbolButtonUp(i - 8);
        }
        // else if(i >= 12 && i < 16)
        // {
        //     return Input.GetButtonUp("FacialPattern4") || SymbolButtonUp(i - 12);
        // }

        return false;
    }

    // 記号のボタンに関する入力を扱うメソッド。
    public static bool SymbolButtonDown(int i)
    {
        // 0 : ■ , 1 : × , 2 : ● , 3 : ▲
        return Input.GetKeyDown(SymbolButtonName(i));
    }

    public static bool SymbolButtonUp(int i)
    {
        // 0 : ■ , 1 : × , 2 : ● , 3 : ▲
        return Input.GetKeyUp(SymbolButtonName(i));
    }

    private static string SymbolButtonName(int i)
    {
        if(i >= 0 && i < 4)
        {
            return "joystick button " + i.ToString();
        }
        else
        {
            Debug.LogError("不正な入力です。");
            return "";
        }
    }
}
