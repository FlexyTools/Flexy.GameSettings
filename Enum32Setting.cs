using System.Runtime.InteropServices;

namespace Flexy.GameSettings;

public	struct Enum32Setting<T> : IClearable where T: unmanaged, Enum, IComparable
{
	public Enum32Setting ( String key, T defaultValue, Boolean readLater = false )
	{
		_key        = $"Flexy.GameSettings  Enum {typeof(T).Name} " + key;
		_default    = defaultValue;
		_value      = readLater ? _default : Ev.VtoT( SGS.Serializer.GetInt(_key, Ev.TtoV( defaultValue )) );
		Changed		= null;
	}

	private	readonly	String	_key;
	private	readonly	T		_default;
	private				T		_value;

	public event		Action<T> Changed;
	
	public	Boolean	HasValue	=> SGS.Serializer.HasKey(_key);
	
	public	T		Read		( ) => _value = Ev.VtoT( SGS.Serializer.GetInt(_key, Ev.TtoV( _default )) );
	public	T		Get			( )
	{ 
		return _value;
	}
	public	void	Set			( T value )
	{ 
		if (value.CompareTo( _value ) == 0)
			return;

		_value = value;
		SGS.Serializer.SetInt(_key, Ev.TtoV(value));

		try						{ Changed?.Invoke(value); }
		catch (Exception ex)	{ Debug.LogException(ex); }
	}
	public	void	SetDefault	( ) => Set(_default);

	public static	implicit operator T ( Enum32Setting<T> @this ) => @this._value;

	[StructLayout(LayoutKind.Explicit)]
	private ref struct Ev
	{
		[FieldOffset(0)] public T		Enum;
		[FieldOffset(0)] public Int32	Value;
	
		public static	Int32	TtoV( T t )			=> new Ev { Enum = t }.Value;
		public static	T		VtoT( Int32 v )		=> new Ev { Value = v }.Enum;
	}
}