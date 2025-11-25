using openGL2.Shaders;
using openGL2.Shaders.ShaderComAndElements;
using OpenTK.Mathematics;



namespace openGL2.Objects.Scenes
{
    internal class GrassScene : Scene
    {
        AnimatedGrass _animatedGrass = new AnimatedGrass();
        Figure grass;
        int oldamounty;
        int oldAmountx;

        public override void OpenScene()
        {
            Load();
        }

        List<Figure> grassElements = new();
        Plane terrainVInfo = new();
        public override void Load()
        {
            ShaderElementBase pos = new PositionVertexShader();
            ShaderElementBase shitShowVertex = new ShitShowVertexShader();
            ShaderElementBase endVertex = new VertexShaderEnd();
            ShaderElementBase overrideFragmentShader = new FragmentShaderOverrider();
            ShaderElementBase simpelFrag = new SimpleColorFragmentShader();
            ShaderElementBase grassFrag = new ShaderElementFragmentGrassMaker();

            Shader terrainShader = new([pos, shitShowVertex, endVertex, overrideFragmentShader, simpelFrag]);
            Shader shaderProgram = new([pos, shitShowVertex, _animatedGrass, overrideFragmentShader, grassFrag]);

            GrassObject grassVInfo = new();

            Figure terrain = new([terrainShader], terrainVInfo, terrainVInfo);
            grass = new([shaderProgram], grassVInfo);

            int xAmount = terrainVInfo.SubdivideWidth;
            int yAmount = terrainVInfo.SubdivideHeight;
            float width = (float) terrainVInfo.Width / xAmount;
            float height = (float) terrainVInfo.Height / yAmount;

            oldAmountx = xAmount;
            oldamounty = yAmount;

            Random rand = new Random();

            for (int x = 0; x < xAmount; x++)
            {
                for (int y = 0; y < yAmount; y++)
                {
                    Matrix4 randomRotation = Matrix4.CreateRotationY((float) rand.NextDouble() * 4);
                    Matrix4 translateion = Matrix4.CreateTranslation(new Vector3(width * x, 0, height * y));
                    Matrix4 randomScale = Matrix4.CreateScale(rand.Next(5,15) * 0.1f);

                    Figure newGrass = grass.GetDublicate();
                    newGrass.TranslateFigure(randomRotation * randomScale * translateion);
                    grassElements.Add(newGrass);
                }
            }

            terrainVInfo.OnUIChange += UpdateTerrain;
        }

        public void UpdateTerrain(object sender, EventArgs e)
        {
            int xAmount = oldAmountx;
            int yAmount = oldamounty;
            float width = (float)terrainVInfo.Width / xAmount;
            float height = (float)terrainVInfo.Height / yAmount;

            Random rand = new Random();

            {
                for (int x = 0; x < xAmount; x++)
                {
                    for (int y = 0; y < yAmount; y++)
                    {
                        Matrix4 randomRotation = Matrix4.CreateRotationY((float)rand.NextDouble() * 4);
                        Matrix4 translateion = Matrix4.CreateTranslation(new Vector3(width * x, 0, height * y));
                        Matrix4 randomScale = Matrix4.CreateScale(rand.Next(5, 15) * 0.1f);

                        grassElements[yAmount * y + x].SetModelSpace(randomRotation * randomScale * translateion);

                    }
                }
            }
        }

        float _timeCounter;

        public override void OnRenderFrame()
        {
            _timeCounter += 0.0001f;
            _animatedGrass.UpdateTime(_timeCounter);

        }


    }
}
