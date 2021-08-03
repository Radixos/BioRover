using UnityEngine;
using UnityEngine.Rendering;

namespace Scanner
{
	public class Matrix : MonoBehaviour
	{
		public Camera m_Cam;
		public Texture m_Font;
		public ComputeShader m_NoiseGenerator;
		public RenderTexture m_RtNoise;

		void Start()
		{
			m_RtNoise = new RenderTexture(512, 512, 0)
			{
				name              = "noise",
				enableRandomWrite = true,
				useMipMap         = false,
				filterMode        = FilterMode.Point,
				wrapMode          = TextureWrapMode.Repeat
			};
			m_RtNoise.Create();
			m_NoiseGenerator.SetTexture(0, "_WhiteNoise", m_RtNoise);
			Shader.SetGlobalTexture("_GlobalFont", m_Font);
			Shader.SetGlobalTexture("_GlobalNoise",  m_RtNoise);
		}
		void Update()
		{
			m_NoiseGenerator.SetInt("_RandSeed", Mathf.CeilToInt(Time.time * 6.0f));
			m_NoiseGenerator.Dispatch(0, 512 / 8, 512 / 8, 1);
		}
	}
}