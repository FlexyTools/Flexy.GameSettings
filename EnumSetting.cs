using System.Runtime.InteropServices;

namespace Flexy.GameSettings;

public		struct EnumSetting<T> where T: Enum, IComparable
{
	public EnumSetting ( String key, T defaultValue )
	{
		_key        = $"Flexy.GameSettings  Enum {typeof(T).Name} " + key;
		_default    = defaultValue;
		_value      = Union.VtoT( GameSettingsService.Serializer.GetInt(_key, Union.TtoV( defaultValue )) );
		Changed		= null;
	}

	private	String	_key;
	private	T		_default;
	private	T		_value;
	
	public	Boolean	HasValue => GameSettingsService.Serializer.HasKey( _key );

	public event	Action<T> Changed;
	
	public T Get	( )
	{ 
		return _value;
	}
	public void Set	( T value )
	{ 
		if ( value.CompareTo( _value ) == 0 )
			return;

		_value = value;
		GameSettingsService.Serializer.SetInt( _key, Union.TtoV(value) );

		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}

	public static	implicit operator T ( EnumSetting<T> @this ) => @this._value;
	
	[StructLayout(LayoutKind.Explicit, Pack = 0)]
	private struct Union
	{
		[FieldOffset(0)] public Int32	Value;
		[FieldOffset(0)] public T		Enum;
		
		public static Int32	TtoV( T t )		=> new Union { Enum	 = t }.Value;
		public static T		VtoT( Int32 v )	=> new Union { Value = v }.Enum;
	}
}