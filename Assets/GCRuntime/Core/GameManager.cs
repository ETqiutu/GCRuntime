using GCRuntime.Utility;
using System.Collections.Generic;

namespace GCRuntime
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private readonly List<IGameEntry> _modules = new List<IGameEntry>();
        private bool _isRunning = false;

        /// <summary>
        /// 注册模块
        /// </summary>
        public void Register(IGameEntry module)
        {
            if (module == null || _modules.Contains(module)) return;
            
            _modules.Add(module);
            if (_isRunning)
                module.OnGameStart();
        }

        /// <summary>
        /// 批量注册
        /// </summary>
        public void Register(params IGameEntry[] modules)
        {
            foreach (var m in modules)
                Register(m);
        }

        /// <summary>
        /// 注销模块
        /// </summary>
        public void Unregister(IGameEntry module)
        {
            if (module == null) return;
            
            if (_isRunning)
                module.OnGameQuit();
                
            _modules.Remove(module);
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        public void StartGame()
        {
            if (_isRunning) return;
            
            _isRunning = true;
            foreach (var m in _modules)
                m.OnGameStart();
        }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        public void PauseGame()
        {
            foreach (var m in _modules)
                m.OnGamePause();
        }

        /// <summary>
        /// 恢复游戏
        /// </summary>
        public void ResumeGame()
        {
            foreach (var m in _modules)
                m.OnGameResume();
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void QuitGame()
        {
            if (!_isRunning) return;
            
            _isRunning = false;
            foreach (var m in _modules)
                m.OnGameQuit();
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        public T GetModule<T>() where T : class, IGameEntry
        {
            foreach (var m in _modules)
            {
                if (m is T result)
                    return result;
            }
            return null;
        }

        // Unity 生命周期驱动
        private void Start() => StartGame();
        private void Update()
        {
            if (!_isRunning) return;
            foreach (var m in _modules)
                m.OnGameUpdate();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause) PauseGame();
            else ResumeGame();
        }

        protected override void OnDestroy()
        {
            QuitGame();
            base.OnDestroy();
        }
    }
}