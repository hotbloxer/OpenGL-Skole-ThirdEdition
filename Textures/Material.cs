using openGL2.Shaders;
using OpenTK.Graphics.OpenGL4;


namespace openGL2.Textures
{
    public class Material : IMaterial
    {
        private Texture _albedoTexture;
        public Texture Albedo { get => _albedoTexture; set => _albedoTexture = value; }

        private Texture _lightMap;
        public Texture LightMap { get => _lightMap; set => _lightMap = value; }

        private Texture _specularTexture;
        public Texture SpecularMap { get => _specularTexture; set => _specularTexture = value; }

        private Texture _normalTexture;
        public Texture NormalTexture { get => _normalTexture; set => _normalTexture = value; }



        public Material ()
        {
            _albedoTexture = GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.WHITE);
            _lightMap = GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.WHITE);
            _specularTexture = GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.WHITE);
            _normalTexture = GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.WHITE);

        }


        Material IMaterial.Material { get => this; set => SetMaterial(value); }

        private void SetMaterial(IMaterial newMaterial)
        {
            _albedoTexture = newMaterial.Albedo;
            _lightMap = newMaterial.LightMap;
            _specularTexture = newMaterial.SpecularMap;
            _normalTexture = newMaterial.NormalTexture;
        }

        public void ActivateMaterial(Shader shader)
        {
            GL.BindTextureUnit((int)Shader.TextureUnits.ALBEDO, _albedoTexture.ID);
            shader.SetTextureUniform(Shader.TextureUnits.ALBEDO);

            GL.BindTextureUnit((int)Shader.TextureUnits.LIGHTMAP, _lightMap.ID);
            shader.SetTextureUniform(Shader.TextureUnits.LIGHTMAP);

            GL.BindTextureUnit((int)Shader.TextureUnits.SPECULARMAP, _specularTexture.ID);
            shader.SetTextureUniform(Shader.TextureUnits.SPECULARMAP);

            GL.BindTextureUnit((int)Shader.TextureUnits.NORMALMAP, _normalTexture.ID);
            shader.SetTextureUniform(Shader.TextureUnits.NORMALMAP);

            
        }
    }

    public interface IMaterial
    {
        public Material Material { get; set; }
        public Texture Albedo { get; set; }
        public Texture LightMap { get; set; }
        public Texture SpecularMap { get; set; }
        public Texture NormalTexture { get; set; }
    

        public void ActivateMaterial(Shader shader);

    }
}
