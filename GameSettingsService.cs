#if FLEXY_GAMEWORLD
namespace Flexy.GameSettings
{
	public class GameSettingsService : GameWorlds.ServiceProvider
	{
		public override void ProvideServices( GameWorlds.GameContext world )
		{
			var settings = new GameSettings( );
			settings.Init( );
			
			world.SetService( settings );
		}
	}
}
#endif