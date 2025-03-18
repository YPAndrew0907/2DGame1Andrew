using System.Collections;
using Cysharp.Threading.Tasks;
using Mgr;
using UnityEngine;
using XYZFrameWork.Base;

namespace XYZFrameWork
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseWindow  : BaseMono
    {
        #region private Member
        private const float FadeTime = 0.6f;
        private CanvasGroup _canvasGroup;
        private bool _isClosed = true;
        #endregion

        #region private Method

        IEnumerator FadeIn()
        {
            float elapsed    = 0f;
            var   startAlpha = _canvasGroup.alpha;
            var   needTime   = FadeTime * startAlpha;

            while (elapsed < FadeTime)
            {
                elapsed            += Time.deltaTime;
                _canvasGroup.alpha =  Mathf.Lerp(startAlpha, 0, elapsed / needTime);
                yield return null;
            }

            _canvasGroup.alpha          = 0f;
            _canvasGroup.interactable   = false;
            _canvasGroup.blocksRaycasts = false;
        }

        IEnumerator FadeOut()
        {
            float elapsed    = 0f;
            var   startAlpha = _canvasGroup.alpha;
            var   needTime   = FadeTime * (1 - startAlpha);

            while (elapsed < FadeTime)
            {
                elapsed            += Time.deltaTime;
                _canvasGroup.alpha =  Mathf.Lerp(startAlpha, 1, elapsed / needTime);
                yield return null;
            }

            _canvasGroup.alpha          = 1f;
            _canvasGroup.interactable   = true;
            _canvasGroup.blocksRaycasts = true;
        }

        #endregion
        
        #region interface Member


        void Create()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable   = false;
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
            transform.SetAsLastSibling();
            OnCreate();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isFade">是否渐显</param>
        async void Show(bool isFade)
        {
            OnShow(isFade);
            if (_isClosed)
            {
                gameObject.SetActive(true);
                if (isFade)
                {
                    UIMgr.Instance.StartCoroutine(FadeOut());
                    await UniTask.WaitForSeconds(FadeTime);
                }
                _canvasGroup.alpha = 1;
                _canvasGroup.interactable   = true;
                _isClosed = false;
            }
        }

        async void Hide(bool isFade)
        {
            if (!_isClosed)
            {
                OnHide(isFade);
                if (isFade)
                {
                    UIMgr.Instance.StartCoroutine(FadeIn());
                    await UniTask.WaitForSeconds(FadeTime);
                }

                _canvasGroup.alpha        = 0;
                _canvasGroup.interactable = false;
                gameObject.SetActive(false);
                _isClosed = true;
            }
        }

        /// <summary>
        /// 关闭一个窗口
        /// </summary>
        /// <param name="isDestroy"></param>
        /// <param name="isFade"></param>
        async void Close(bool isDestroy , bool isFade)
        {
            if (!_isClosed)
            {
                OnClose();
                if (isFade)
                {
                    UIMgr.Instance.StartCoroutine(FadeIn());
                    await UniTask.WaitForSeconds(FadeTime);
                }
                _canvasGroup.alpha        = 0;
                _canvasGroup.interactable = false;
                gameObject.SetActive(false);
                _isClosed = true;
                if (isDestroy)
                {
                    Destroy(gameObject, 0.5f);
                }
            }
        }

         #endregion

        #region abstract & virtual Member
        public abstract string WindowName { get;}
        
        /// <summary>
        /// 创建一个窗口时, 只会在第一次打开后调用 ，这里创建 mediator
        /// </summary>
        protected abstract void OnCreate();

        /// <summary>
        /// 打开一个窗口 ，这里调用下 mediator 的打开方法
        /// </summary>
        /// <param name="data"> 打开窗口所需的数据</param>
        /// <param name="isFade">是否渐显</param>
        protected abstract void OnOpen(object data, bool isFade);
        
        protected virtual void OnShow(bool isFade = true){ }

        protected virtual void OnHide(bool isFade = true) { }

        protected virtual void OnClose(bool isFade =true) { }

        #endregion
    }

    public interface IWindow
    {
        public string WindowName { get; }
        UIMgr.UILayer Layer      { get; set; }

        void Create();
        void Open(object data,      bool isFade= true);
        void Close(bool  isDestroy, bool isFade = true);
        void Show(bool   isFade = true);
        void Hide(bool   isFade = true);
    }
}