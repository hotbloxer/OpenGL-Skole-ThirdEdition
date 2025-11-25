using ImGuiNET;
using openGL2.Objects;
using openGL2.Shaders.ShaderComAndElements;
using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Text;
using System.Xml.Linq;



namespace openGL2.Shaders
{



    public class Shader : IDisposable
    {

        public string FragmentShaderSource { get; private set; }
        public string VertexShaderSource  { get; private set; }

        public string GeometryShaderSource { get; private set; }

        public int ShaderProgramHandle { get; private set; }

        protected List<ShaderPart> parts = new List<ShaderPart>();
        // den her samler alle shaders

        private Matrix4Uniform _view;
        private Matrix4Uniform _model;
        private Matrix4Uniform _modelView;
        private Matrix4Uniform _projection;
        private Matrix4Uniform _projectionViewModel;

        public Matrix4 View { get => _view.Matrix; set => _view.Matrix = value;}
        public Matrix4 Model{ get => _model.Matrix; set =>_model.Matrix = value; }
        public Matrix4 ModelView { get => _modelView.Matrix; set => _modelView.Matrix = value; }
        public Matrix4 Projection { get => _projection.Matrix; private set => _projection.Matrix = value; }


        ShaderCombiner sc;

        public Dictionary<uint, ShaderElementBase> ShaderScripts = new();

        public Dictionary<uint, ShaderElementBase> geometryShaders = new();

        public bool UsesGeometryShader = false;
        public bool usesMaterial = false;

        ShaderElementBase sunFrag = new FragmentShaderSun();

        public Shader (ShaderElementBase[] shaderElements)
        {
            sc = new ShaderCombiner(this);

            foreach (ShaderElementBase element in shaderElements)
            {
                ShaderScripts.Add(element.id, element);
                sc.elements.Add(element);

                if (element.ShaderType == ShaderType.GeometryShader)
                {
                    geometryShaders.Add(element.id, element);
       
                }
                 
                if (element is IUseMaterial)
                {
                    usesMaterial = true;
                }
      
            }
       
            if (geometryShaders.Count > 0)
            {

            SetActiveGeometryShader(geometryShaders.FirstOrDefault().Key);
            }


            FragmentShaderSource = SetDefaultFragmentShader();
            VertexShaderSource = SetDefaultVertexShader();
            GeometryShaderSource = SetDefaultGeometryShader();



            ShaderProgramHandle = GL.CreateProgram();

            _view = new Matrix4Uniform(ShaderProgramHandle, "view");
            _model = new Matrix4Uniform(ShaderProgramHandle, "model");
            _modelView = new Matrix4Uniform(ShaderProgramHandle, "modelView");
            _projection = new Matrix4Uniform(ShaderProgramHandle, "projection");
            _projectionViewModel = new Matrix4Uniform(ShaderProgramHandle, "projectionViewModel");

            // adds vertex and fragments to list, da der skal udføres de samme operationer
            // dette reducere dublikationer og der kan tilføjes flere slags shaders senere
            parts.Add(new ShaderPart(ShaderType.VertexShader, VertexShaderSource));
            parts.Add(new ShaderPart(ShaderType.FragmentShader, FragmentShaderSource));
            parts.Add(new ShaderPart(ShaderType.GeometryShader, GeometryShaderSource));



            UseShaderSetup();
           

            GL.LinkProgram(ShaderProgramHandle);

            // test for link fejl
            GL.GetProgram(ShaderProgramHandle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(ShaderProgramHandle);
                Console.WriteLine(infoLog);
            }
            DetachAndDeleteShaderParts();

            ShaderHandler.AddShader(this);
        }

        
        public bool IsUpToDate = false;
        public void UseShader ()
        {
            if (!IsUpToDate)
            {
                UseShaderSetup();
                IsUpToDate = true;
            }

            else
            {
                GL.UseProgram(ShaderProgramHandle);
            }
        }

        public void UseShaderSetup ()
        {
            foreach (ShaderPart part in parts)
            {
                if (part.type == ShaderType.FragmentShader)
                {
                    FragmentShaderSource = SetDefaultFragmentShader();
                    part.shaderSource = FragmentShaderSource;
                }
                else if (part.type == ShaderType.VertexShader)
                {
                    VertexShaderSource = SetDefaultVertexShader();
                    part.shaderSource = VertexShaderSource;
                }
                else if (part.type == ShaderType.GeometryShader)
                {
                    GeometryShaderSource = SetDefaultGeometryShader();
                    part.shaderSource = GeometryShaderSource;
                }
            }


            UpdateShaderParts();

            GL.LinkProgram(ShaderProgramHandle);
        }

        public Dictionary<uint, ShaderElementBase> GetGeometryShaders ()
        {
            return geometryShaders;
        }
        public void UpdateUniforms ()
        {
            
            sc.SetUniforms(UsesGeometryShader);
        }
        


        private string SetDefaultGeometryShader ()
        {
            string layout = sc.GetLayouts(ShaderType.GeometryShader);
            string code = sc.GetShaderCode(ShaderType.GeometryShader);
            string functions = sc.GetFunctions();
            string uniforms = sc.GetShaderUniforms(ShaderType.GeometryShader);

            

            if (layout.Length == 0)
            {
                layout = @"
                layout(triangles) in;
                layout(line_strip, max_vertices = 5) out;";
            }

            

            return $@"
                #version 330 core

                {layout}

                {uniforms}
                
                {functions}

                void main()
                {{
                
                    {code}

                }}
            ";
        }

        private string SetDefaultVertexShader ()
        {
            string layouts = sc.GetLayouts(ShaderType.VertexShader);
            string uniforms = sc.GetShaderUniforms(ShaderType.VertexShader);
            string code = sc.GetShaderCode(ShaderType.VertexShader);


            return
            @$"#version 330 core 
            {layouts}
            {uniforms}

            void main() 
            {{ 


     
                {code}
        


            }}";

        }

        private bool isUsingDefaultFragmentShader = true;

        #region FRAGMENT SHADER
        private string SetDefaultFragmentShader()
        {
            string layouts = "";
            string uniforms = "";
            string code = "";



            if (sc.OverrideFragmentShader())
            {

                isUsingDefaultFragmentShader = false;
                layouts = sc.GetLayouts(ShaderType.FragmentShader); ;
                uniforms = sc.GetShaderUniforms(ShaderType.FragmentShader);
                code = sc.GetShaderCode(ShaderType.FragmentShader);


                return @$"#version 330 core

                {uniforms}
                {layouts}

                void main()
                {{ 
                    
                {code}
                
                }}
                "
                ;

            }
            
         
            uniforms = "";
            code = sc.GetShaderCode(ShaderType.FragmentShader);


            if (!sc.elements.Contains(sunFrag))
                sc.elements.Add(sunFrag);
            uniforms = sc.GetShaderUniforms(ShaderType.FragmentShader);



            return 
            @$"#version 330 core 
            out vec4 FragColor; 
            in vec2 uv;
            in vec3 vertexNormal;
            in vec3 fragPosition;
            in mat3 tbn;
            {uniforms}


            in vec4 viewPosition;
            

            uniform sampler2D albedoTexture;
            uniform sampler2D lightmapTexture;
            uniform sampler2D normalTexture;
            uniform sampler2D specularTexture;
            

            uniform vec3 {cameraPosition};
            

            // ui ting
            uniform bool {uvTesting};
            uniform bool {useTexture};
            uniform bool {useBlinn};
            uniform bool {usingRimLight};
            uniform bool {useNormalMap};
            
            uniform vec3 {lightColor};
            uniform vec3 {objectColor};

                    
            
            vec3 lightPosition = lightPositionIn;
            vec3 rimColor = vec3 (1, 1, 1);
          
            float ambientStrength = 0.1f;

            void main() 
            {{
      
            vec3 oldLight = lightColor;
            vec3 lightcolNew = sunLight ;

            vec3 normal;

            if ({useTexture}) 

            {{


                if ({useNormalMap}) 
                {{
                    normal = vertexNormal;
                }}

                else 
                {{
                    //hent normal i range 0 - 1            
                    normal = texture(normalTexture, uv).rgb;
                    // lav den til -1 til 1
                    normal = normalize(normal * 2.0 - 1.0); 

                     normal =  normalize (tbn * normal);

                }}
            

            }} 
            else 
            {{
                normal = vertexNormal;
            }}  

            


            //base color
            vec4 pixel = vec4({objectColor},1);


            if ({useTexture}) 
            {{pixel = vec4 (texture(albedoTexture, uv).rgba);
                if (pixel.w < 0.1) discard;
            }}
            

            // ambient light
            vec4 ambient;
            vec4 LightTexPixel = texture(lightmapTexture, uv);

           
            if ({useTexture}) 
            {{
                ambient  = vec4((ambientStrength * lightcolNew), 1) * LightTexPixel;    
            }}
           
            else 
            {{
               ambient = vec4(ambientStrength * lightcolNew, 1);
            }}
  
            

            // diffuse light
             
            vec3 normalizedNormal = normalize(normal);
            vec3 lightDir = normalize(lightPosition - fragPosition); 
            float diff = max(dot(normalizedNormal, lightDir), 0.0);
            vec4 diffuse = vec4(diff * lightcolNew, 1);
            


            // specular
            float specularStrength = 0.3;
            vec3 viewDir = normalize({cameraPosition} - fragPosition);

            float specValue = 0.0;
            // blinn shader
            if ({useBlinn})
            {{
               
                vec3 halfwayVector = normalize(lightDir + viewDir);
                specValue = pow(max(dot(normalizedNormal, halfwayVector), 0.0), 4);

            }}
            else 
            {{
                vec3 reflectDir = reflect(-lightDir, normalizedNormal); 
                specValue = pow(max(dot(viewDir, reflectDir), 0.0), 32);
            }}


            vec4 specular;
            vec3 specularTexPixel = texture(specularTexture, uv).rgb;
            
            
            if ({useTexture}) 
            {{
                
                specular = vec4 ((specularStrength * specValue * lightcolNew) * specularTexPixel, 1)   ;  
            }}
           
            else 
            {{
                specular = vec4 (specularStrength * specValue * lightcolNew , 1); 
            }}
            

            float rimLight = 0;
            if ({usingRimLight}) 
            {{
                rimLight = 1.0f - max(dot(viewDir, normalizedNormal), 0.0f);
                rimLight = smoothstep(0.85, 1, rimLight); 
            }}


            // afsluttende udregning
            FragColor = (ambient + diffuse + specular) * pixel + (rimLight * 0.8);
     
            

            // decals / projected textures
            //vec2 projectTexCoord; 
            //projectTexCoord.x= viewPosition.x/ viewPosition.w * 0.5 + 0.5;
            //projectTexCoord.y= viewPosition.y/ viewPosition.w * 0.5 + 0.5; 

            //if ((clamp(projectTexCoord.x, 0.0, 1.0) == projectTexCoord.x) &&
            //    (clamp(projectTexCoord.y, 0.0, 1.0) == projectTexCoord.y)) 
            //{{
            //     FragColor= texture(specularTexture, projectTexCoord); 
            //}}


            {code}




            // from below here is testing area
                
            // uv test
            if ({uvTesting})
            {{ FragColor = vec4(uv, 0.0, 1.0); }}

            // afslutning
            }}";
        }

        #endregion

        

        protected void SetUpShaderParts()
        {
            foreach (ShaderPart part in parts)
            {
                if (!UsesGeometryShader && part.type == ShaderType.GeometryShader)
                {
                    continue;
                }


                part.shaderPartHandle = GL.CreateShader(part.type);

                GL.ShaderSource(part.shaderPartHandle, part.shaderSource);

                GL.CompileShader(part.shaderPartHandle);
                GL.GetShader(part.shaderPartHandle, ShaderParameter.CompileStatus, out var code);
                if (code != 1)
                {


                    var infoLog = GL.GetShaderInfoLog(part.shaderPartHandle);
                throw new Exception($"Fejl i shader ({part.shaderPartHandle}).\n\n{infoLog}");
                }
                GL.AttachShader(ShaderProgramHandle, part.shaderPartHandle);
            }
        }

     

        private void UpdateShaderParts()
        {
         

            foreach (ShaderPart part in parts)
            {
                GL.DetachShader(ShaderProgramHandle, part.shaderPartHandle);
                GL.DeleteShader(part.shaderPartHandle);
            }
            SetUpShaderParts();
         

        }


        public ShaderElementBase SetActiveGeometryShader(uint id) 
        { 
            foreach (ShaderElementBase geoShader in geometryShaders.Values)
            {
                geoShader.Apply = false;
            }

            geometryShaders[id].Apply = true;
            return geometryShaders[id];

        }


        // UI inputs
        readonly string uvTesting = "uvTesting";
        public void SetUVTest(bool state)
        {
            SetUniformBool("uvTesting", state);
        }

        readonly string useTexture = "useTexture";
        public void SetUsingTexture(bool state)
        {
            SetUniformBool(useTexture, state);
        }

        readonly string useNormalMap = "useNormalMap";
        public void SetUsingNormalMap (bool state)
        {
            SetUniformBool(useNormalMap, state);
        }

        readonly string useBlinn = "useBlinn";
        public void SetUsingBlinn (bool state)
        {
            SetUniformBool(useBlinn, state);
        }

        readonly string cameraPosition = "cameraPosition";
        public void SetCameraPosition(Vector3 position)
        {
            SetUniformVector3(cameraPosition, position);
        }

        readonly string usingRimLight = "usingRimLight";
        public void UsingRimLight(bool state)
        {
            SetUniformBool(usingRimLight, state);
        }

        readonly string lightColor = "lightColor";
        public void SetLightColor (Vector3 color)
        {
            SetUniformVector3(lightColor, color);
        }

        readonly string objectColor = "objectColor";
        public void SetObjectColor (Vector3 color)
        {
            SetUniformVector3(objectColor, color);
        }

        public void Dispose()
        {
            ShaderHandler.RemoveShader(this);
        }

        public void UpdateUniformValuesForRender()
        {
            // det her regnes bagfra?? TODO Spørg Søren
            _projectionViewModel.Matrix = _model.Matrix * _view.Matrix * _projection.Matrix;
        }

        /// <summary>
        /// opdater alle views i alle de forskellige shaders på en gang
        /// </summary>
        /// <param name="view"></param
        /// 
        public static void UpdateView(Matrix4 view)
        {
            foreach (Shader shader in ShaderHandler.GetShaders().Values)
            {
                shader.View = view;
            }
        }

        public static void UpdateCameraPosition(Vector3 cameraPosition)
        {
            foreach (Shader shader in ShaderHandler.GetShaders().Values)
            {
                shader.SetCameraPosition(cameraPosition);
            }
        }

        protected void SetUniformBool(string uniformName, bool boolStatus)
        {
            if (!isUsingDefaultFragmentShader) return;

            Use();
            int location = GL.GetUniformLocation(ShaderProgramHandle, uniformName);
            if (location == -1)
                throw new Exception($"Uniform '{uniformName}' not found.");

            GL.Uniform1(location, boolStatus ? 1 : 0);
        }

        protected void SetUniformVector3(string uniformName, Vector3 vector)
        {
            if (!isUsingDefaultFragmentShader) return;
            Use();
            int location = GL.GetUniformLocation(ShaderProgramHandle, uniformName);
            if (location == -1)
                throw new Exception($"Uniform '{uniformName}' not found.");

            GL.Uniform3(location, vector);
        }

        protected void SetUniformVector4(string uniformName, Vector4 vector)
        {
            Use();
            int location = GL.GetUniformLocation(ShaderProgramHandle, uniformName);
            if (location == -1)
                throw new Exception($"Uniform '{uniformName}' not found.");

            GL.Uniform4(location, vector);
        }

        public static void UpdateProjection(Matrix4 projection)
        {
            foreach (Shader shader in ShaderHandler.GetShaders().Values)
            {
                shader.Projection = projection;
            }
        }

        public static void UpdateModelSpace(Matrix4 modelSpace)
        {
            foreach (Shader shader in ShaderHandler.GetShaders().Values)
            {
                shader.Model = modelSpace;

                shader.CalculateModelView();
            }
        }


        public void CalculateModelView()
        {
            ModelView = _view.Matrix * _model.Matrix;
        }


        public enum TextureUnits { ALBEDO, LIGHTMAP, SPECULARMAP, NORMALMAP }

        public void SetTextureUniform(TextureUnits textureUnit) 
        {
            string uniformName = string.Empty;
            switch (textureUnit)
            {
                case TextureUnits.ALBEDO:
                    uniformName = "albedoTexture";
                    break;

                case TextureUnits.LIGHTMAP:
                    uniformName = "lightmapTexture";
                    break;

                case TextureUnits.SPECULARMAP:
                    uniformName = "specularTexture";
                    break;

                case TextureUnits.NORMALMAP:
                    uniformName = "normalTexture";
                    break;
            }

            Use();
            int location = GL.GetUniformLocation(ShaderProgramHandle, uniformName);
            if (location == -1)
                throw new Exception($"Uniform '{uniformName}' not found.");

            GL.Uniform1(location, (int)textureUnit);
        }

        public void Use()
        {
            GL.UseProgram(ShaderProgramHandle);
        }

        protected void DetachAndDeleteShaderParts()
        {
            foreach (ShaderPart part in parts)
            {
                GL.DetachShader(ShaderProgramHandle, part.shaderPartHandle);
                GL.DeleteShader(part.shaderPartHandle);
            }
        }

        public Shader Dublicate()
        {
            ShaderElementBase[] elements = new ShaderElementBase[ShaderScripts.Count];
            int i = 0;
            foreach (ShaderElementBase element in ShaderScripts.Values)
            {
                elements[i] = element;
                i++;
            }
            Shader newShader = new Shader(elements);
            return newShader;
        }
    }

    public class ShaderPart
    {
        public ShaderType type;
        public int shaderPartHandle = -1;
        public string shaderSource;

        public ShaderPart(ShaderType type, string shaderSource)
        {
            this.type = type;
            this.shaderSource = shaderSource;
        }
    }


    public abstract class UniformElement 
    {
        public readonly string Name;
        public readonly string UniformType;


        public UniformElement (string name, string uniformType)
        {
            Name = name;
            UniformType = uniformType;
        }

        public abstract void SetUniform(int ShaderProgramHandle);

    }

    public abstract class LayoutBase
    {
        


        protected LayoutBase()
        {
            
        }

        public virtual string GetLayoutString()
        {
            return "";
        }


}




    






}


