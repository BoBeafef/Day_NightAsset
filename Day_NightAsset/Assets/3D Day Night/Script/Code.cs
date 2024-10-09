using System.Threading.Tasks;
using UnityEngine;
namespace DayNighteCode
{
    [ExecuteInEditMode]
    public class Code : MonoBehaviour
    {
        [SerializeField] private int numerOfStars = 200;
        [SerializeField] private float _dayTime = 20;

        [SerializeField] private Gradient _Sun;
        [SerializeField] private Gradient _Moon;
        [SerializeField, Range(0, 1)] private float _timeProgress;
        [SerializeField] private AnimationCurve _sunCurve;
        [SerializeField] private AnimationCurve _moonCurve;

        [SerializeField] private Material _sky;

        [SerializeField] private Light _sunLight;
        [SerializeField] private Light _moonLight;
        public ParticleSystem stars;

        //public ParticleSystem star;
        private Vector3 _defoltAngles;
        void Start()
        {
            _defoltAngles = _sunLight.transform.localEulerAngles;
        }

        void Update()
        {
            bool isNight = _timeProgress >= 0f && _timeProgress <= 0.25f || _timeProgress >= 0.75f && _timeProgress <= 1f;
            _sunLight.intensity = _sunCurve.Evaluate(_timeProgress);
            _moonLight.intensity = _moonCurve.Evaluate(_timeProgress) / 3;

            if (Application.isPlaying)
            { _timeProgress += Time.deltaTime / _dayTime; }
            if (_timeProgress > 1f)
            {
                _timeProgress = 0f;
            }
            _sunLight.color = _Sun.Evaluate(_timeProgress);

            _sky.SetFloat("_AtmosphereThickness", _sunCurve.Evaluate(_timeProgress));
            _sky.SetFloat("_Exposure", _sunCurve.Evaluate(_timeProgress) / 1.5f);
            RenderSettings.ambientLight = _Sun.Evaluate(_timeProgress);
            RenderSettings.sun = isNight ? _moonLight : _sunLight;

            Task.Factory.StartNew(() =>
            {
                DynamicGI.UpdateEnvironment();
            });
            var starsN = stars.main;
            if (isNight)
            {

                starsN.maxParticles = numerOfStars;
            }
            else
            {
                starsN.maxParticles = 0;

            }
            var _sunT = _timeProgress;
            var _moonT = _timeProgress;
            _sunLight.transform.localEulerAngles = new Vector3(360 * _sunT - 90, _defoltAngles.x, _defoltAngles.y);
            _moonLight.transform.localEulerAngles = new Vector3(360 * _moonT + 90, _defoltAngles.x, _defoltAngles.y);


        }
    }
}

