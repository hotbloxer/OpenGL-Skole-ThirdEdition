using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


namespace openGL2.Shaders
{
    public class Shader : IDisposable
    {
        //REFA bør nok have en sparat manger til det her engang
        public static List<Shader> shaders = new List<Shader>();

        private string _vertexShaderSource;
        private string _fragmentShaderSource;

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

        public Shader() 
        {
            _fragmentShaderSource = SetDefaultFragmentShader();
            _vertexShaderSource = SetDefaultVertexShader();

            ShaderProgramHandle = GL.CreateProgram();

            _view = new Matrix4Uniform(ShaderProgramHandle, "view");
            _model = new Matrix4Uniform(ShaderProgramHandle, "model");
            _modelView = new Matrix4Uniform(ShaderProgramHandle, "modelView");
            _projection = new Matrix4Uniform(ShaderProgramHandle, "projection");
            _projectionViewModel = new Matrix4Uniform(ShaderProgramHandle, "projectionViewModel");

            // adds vertex and fragments to list, da der skal udføres de samme operationer
            // dette reducere dublikationer og der kan tilføjes flere slags shaders senere
            parts.Add(new ShaderPart(ShaderType.VertexShader, _vertexShaderSource));
            parts.Add(new ShaderPart(ShaderType.FragmentShader, _fragmentShaderSource));

            SetUpShaderParts();

            GL.LinkProgram(ShaderProgramHandle);

            // test for link fejl
            GL.GetProgram(ShaderProgramHandle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(ShaderProgramHandle);
                Console.WriteLine(infoLog);
            }
            DetachAndDeleteShaderParts();

            shaders.Add(this);
        }



        private static string SetDefaultVertexShader ()
        {
            return
            @$"#version 330 core 
            layout(location = 0) in vec3 aPosition;
            layout(location = 1) in vec2 aUV;
            layout(location = 2) in vec3 aNormal;
            layout(location = 3) in vec3 aTangent;  
            layout(location = 4) in vec3 aBiNormal;

            uniform mat4 modelView;
            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;
            uniform mat4 projectionViewModel;

            uniform mat4 texProjection;

            out vec2 uv;
            out vec3 vertexNormal;
            out vec3 fragPosition;
            out mat3 tbn;

            out vec4 viewPosition; 

            void main() 
            {{ 
                fragPosition = vec3(model * vec4(aPosition, 1.0));
                uv = aUV;

                viewPosition = projection * view * vec4(aPosition, 1.0);

                mat4 modelview2 =   view * model;
                mat3 normalMatrix = transpose(inverse(mat3( modelview2)));
                mat4 modelViewInstance = modelView;


                tbn = mat3
                  (
                     normalize(vec3(model * vec4(aTangent , 0))),
                     normalize(vec3(model * vec4(aBiNormal, 0))),
                     normalize(vec3(model * vec4(aNormal  , 0)))
                  );


                vertexNormal =  aNormal;
                

                mat4 test = view;
                mat4 test2 = model;
                mat4 test3 = projection;


                //gl_Position = projection * view  * model * vec4(aPosition, 1.0) ;
                gl_Position = projectionViewModel * vec4(aPosition, 1.0) ;

            }}";

        }
        private string SetDefaultFragmentShader()
        {
            return 
            @$"#version 330 core 
            out vec4 FragColor; 
            in vec2 uv;
            in vec3 vertexNormal;
            in vec3 fragPosition;
            in mat3 tbn;


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
            uniform bool {usingCellShading};  
            uniform bool {usingRimLight};
            
            uniform vec3 {lightColor};
            uniform vec3 {objectColor};

        
            vec3 lightPosition = vec3 (0, 3, 1);
            vec3 rimColor = vec3 (1, 1, 1);
          
            float ambientStrength = 0.1f;

            void main() 
            {{
      

            vec3 normal;

            if ({useTexture}) 

            {{
            //hent normal i range 0 - 1
            normal = texture(normalTexture, uv).rgb;
            // lav den til -1 til 1
            normal = normalize(normal * 2.0 - 1.0); 

             normal =  normalize (tbn * normal);
            }} 
            else 
            {{
                normal = vertexNormal;
            }}  



            //base color
            vec4 pixel = vec4({objectColor},1);

            vec4 tempPixel = vec4(texture(albedoTexture, uv).rgb, 1.0);

            
            if ({useTexture}) 
            {{pixel = vec4 (texture(albedoTexture, uv).rgb, 1);}}
            

            // ambient light
            vec4 ambient;
            vec4 LightTexPixel = texture(lightmapTexture, uv);

           
            if ({useTexture}) 
            {{
                ambient  = vec4((ambientStrength * lightColor), 1) * LightTexPixel;    
            }}
           
            else 
            {{
               ambient = vec4(ambientStrength * lightColor, 1);
            }}
  
            

            // diffuse light
             
            vec3 normalizedNormal = normalize(normal);
            vec3 lightDir = normalize(lightPosition - fragPosition); 
            float diff = max(dot(normalizedNormal, lightDir), 0.0);
            vec4 diffuse = vec4(diff * lightColor, 1);
            


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
                
                specular = vec4 ((specularStrength * specValue * lightColor) * specularTexPixel, 1)   ;  
            }}
           
            else 
            {{
                specular = vec4 (specularStrength * specValue * lightColor , 1); 
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




            if ({usingCellShading})
            {{
                normal = vertexNormal;
                vec4 color;
                float intensity = dot(lightDir, normal);
                if (intensity > 0.95)
                color = vec4(1.0,0.5,0.5,1.0);
                else if (intensity > 0.5)
                color = vec4(0.6,0.3,0.3,1.0);
                else
                if (intensity > 0.25)
                color = vec4(0.4,0.2,0.2,1.0);
                else
                color = vec4(0.2,0.1,0.1,1.0);
                FragColor = color;
                
            }}

            // from below here is testing area
                
            // uv test
            if ({uvTesting})
            {{ FragColor = vec4(uv, 0.0, 1.0); }}

            // afslutning
            }}";
        }

        protected void SetUpShaderParts()
        {
            foreach (ShaderPart part in parts)
            {
                int shaderToSetUp = GL.CreateShader(part.type);
                GL.ShaderSource(shaderToSetUp, part.shaderSource);

                GL.CompileShader(shaderToSetUp);
                GL.GetShader(shaderToSetUp, ShaderParameter.CompileStatus, out var code);
                if (code != 1)
                {
                    var infoLog = GL.GetShaderInfoLog(shaderToSetUp);
                    throw new Exception($"Fejl i shader ({shaderToSetUp}).\n\n{infoLog}");
                }
                GL.AttachShader(ShaderProgramHandle, shaderToSetUp);
            }
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

        readonly string usingCellShading = "toonShaderOn";
        public void UsingCellShader(bool state)
        {
            SetUniformBool(usingCellShading, state);
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
            shaders.Remove(this);
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
            foreach (Shader shader in shaders)
            {
                shader.View = view;
            }
        }

        public static void UpdateCameraPosition(Vector3 cameraPosition)
        {
            foreach (Shader shader in shaders)
            {
                shader.SetCameraPosition(cameraPosition);
            }
        }

        protected void SetUniformBool(string uniformName, bool boolStatus)
        {
            Use();
            int location = GL.GetUniformLocation(ShaderProgramHandle, uniformName);
            if (location == -1)
                throw new Exception($"Uniform '{uniformName}' not found.");

            GL.Uniform1(location, boolStatus ? 1 : 0);
        }

        protected void SetUniformVector3(string uniformName, Vector3 vector)
        {
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
            foreach (Shader shader in shaders)
            {
                shader.Projection = projection;
            }
        }

        public static void UpdateModelSpace(Matrix4 modelSpace)
        {
            foreach (Shader shader in shaders)
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
    }

    public struct ShaderPart(ShaderType type, string shaderSource)
    {
        public ShaderType type = type;
        public int shaderPartHandle;
        public string shaderSource = shaderSource;
    }
}
