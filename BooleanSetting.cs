namespace Flexy.GameSettings;

public struct BooleanSetting
{
	public BooleanSetting (String key, Boolean defaultValue)
	{
		_key     = "Flexy.GameSettings  Boolean " + key;
		_value   = GameSettings.Serializer.GetBool(_key, defaultValue );
		Changed  = null;
	}

	private String  _key;
	private Boolean _value; 
	
	public event Action<Boolean> Changed;

	public Boolean HasValue => GameSettings.Serializer.HasKey( _key );

	public Boolean Get	( )
	{ 
		return _value;
	}
	public void Set	( Boolean value )
	{ 
		if (_value == value )
			return;

		_value = value;
		GameSettings.Serializer.SetBool(_key, value );

		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}

	public static	implicit operator Boolean ( BooleanSetting @this ) => @this._value;
}