using Flexy.GameWorlds;

namespace Flexy.GameSettings
{
	public class GameSettingsServiceProvider : ServiceProvider
	{
		public override void ProvideServices( GameContext ctx )
		{
			var settings = new GameSettings( );
			settings.Init( );
			
			ctx.SetService( settings );
		}
	}
}
