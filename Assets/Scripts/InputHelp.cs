using UnityEngine;

public class InputHelp 
{
	public static  int GetNumberDown () 
	{
		for ( int i = 0; i < 10; i++ ){
			if (GetKeyDown(i)){
				return i;
			}
		}
		return -1;
	}

	public static bool GetKeyDown ( int n )
	{
		var ns = n.ToString();
		if ( Input.GetKeyDown( ns ) || Input.GetKeyDown("["+ns+"]") )
		{
			return true;
		}
		return false;
	}
}
