namespace GCRuntime
{
    public interface IGameEntry
    {
        void OnGameStart();
        void OnGameUpdate();
        void OnGamePause();
        void OnGameResume();
        void OnGameQuit();
    }
}
