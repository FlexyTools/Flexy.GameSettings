using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Flexy.GameSettings;

public		struct EnumSetting<T> where T: unmanaged, Enum, IComparable
{
	public EnumSetting ( String key, T defaultValue )
	{
		_key        = $"Flexy.GameSettings  Enum {typeof(T).Name} " + key;
		_default    = defaultValue;
		_value      = Union.VtoT( GameSettings.Serializer.GetInt(_key, Union.TtoV( defaultValue )) );
		Changed		= null;
	}

	private	String	_key;
	private	T		_default;
	private	T		_value;
	
	public	Boolean	HasValue => GameSettings.Serializer.HasKey( _key );

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
		GameSettings.Serializer.SetInt( _key, Union.TtoV(value) );

		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}

	public static	implicit operator T ( EnumSetting<T> @this ) => @this._value;
	
	private struct Union
	{
		public T		Enum;
		private Int32	_dummy; // need this for proper struct padding to 4 bytes so then unsafe convert will work properly
		
		public Int32	Value
		{
			get => UnsafeUtility.As<T,Int32>( ref Enum );
			set => Enum = UnsafeUtility.As<Int32, T>( ref value );
		}
		
		public static Int32	TtoV( T t )		=> new Union { Enum	 = t }.Value;
		public static T		VtoT( Int32 v )	=> new Union { Value = v }.Enum;
	}
}