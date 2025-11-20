using ClickableTransparentOverlay;
using ImGuiNET;
using openGL2.Objects;
using openGL2.Shaders;
using openGL2.Shaders.ShaderComAndElements;
using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using System.Windows.Forms;
using static openGL2.Textures.Texture;
using Sys = System.Numerics;
using TK = OpenTK.Mathematics;


namespace openGL2
{

    public class UI 
    {

        // UI input
        public static bool UsingBlinnLight = true; 
        public static bool UsingRimLight = false;
        public static bool DisplayTestingNormals = false;
        public static bool displaUVTesting = false;
        public static bool useTexture = true;
        public static bool useChecker = false;
        public static bool UseEnvironment = true;


        bool _displayLightColorPicker = false;
        bool _displayObjectColorPicker = false;

        Sys.Vector3 _lightColor;
        public static TK.Vector3 LightColorTK = new(1, 1, 1);

        Sys.Vector3 _objectColor = new(1, 1, 1);
        public static TK.Vector3 ObjectColorTK = new(1, 1, 1);

        // bruges til at genbruge color pickereren med
        public delegate void ColorSet(TK.Vector3 color);

        // texture sampling
        string[] _filterTypes = Enum.GetNames(typeof(TextureFilterTypes));

        public IHaveUI windowUI { get; set; }

        //REFA
        // disse styrer texturene og deres filtre
        int _selectedAlbedoFilter = 0;
        int _currentAlbedoTextureIndex = 0;
        bool _anisotropicAlbedo = false; 

        int _currentLightMapTextureIndex = 0;
        int _selectedLightMapFilter = 0;
        bool _anisotropicLightMap = false;

        int _currentSpecularMapTextureIndex = 0;
        int _selectedSpecularMapFilter = 0;
        bool _anisotropicSpeculatMap = false;

        int _currentNormalTextureIndex = 0;
        int _selectedNormalFilter = 0;
        bool _anisotropicNormal = false;

        int _currentHeigtTextureIndex = 0;
        int _selectedHeigtFilter = 0;
        bool _anisotropicHeigt = false;

        int localPlanewidth = 1;
        int localPlaneheight = 1;
        int localPlanesubdivideWidth = 5;
        int localPlanesubdivideHeight = 5;

        ShaderElementBase _selectedGeometryShader;



        // de fleste texturer og andre ting afshænger af hvilken figur der er aktiveret
        int _selectedFigureIndex = 0;
        int _selectedShaderIndex = 0;
        uint _selectedGeoShaderIndex = 0;
 
        Figure _selectedFigure;
        Shader _selectedShader;

        float normalStrength = 0;

        public void RenderView ()
        {
            ImGui.SetNextWindowSize(new Sys.Vector2(400, 800));
            ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.0f, 0.0f, 0.0f, 0.0f)); // Sets window to see through

            // empty window to see through to opengl render
            ImGui.Begin("OpenGL");
            ImGui.PopStyleColor(1);
            ImGui.End();
        }


        public void Ui()
        {

            ImGui.ShowDemoWindow();



            string[] _selectableTextures = Texture.AllTextures.Keys.ToArray();


            ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.0f, 0.0f, 0.0f, 1.0f)); // Back to theme color.. ish
            ImGui.SetNextWindowSize(new Sys.Vector2(400, 800));

            ImGui.Begin("Properties");


            #region Select figure and geometry settings

            _selectedShader ??= ShaderHandler.GetShaders().FirstOrDefault().Value;

            string[] figureNames = ObjectHandler.GetFigures.Keys.ToArray();
            if (ImGui.BeginCombo("Shader", figureNames[_selectedFigureIndex]))
            {
                for (int n = 0; n < figureNames.Length; n++)
                {
                    bool is_selected = _selectedFigureIndex == n;
                    if (ImGui.Selectable(figureNames[n], is_selected))
                    {
                        _selectedFigureIndex = n;
                        _selectedFigure = ObjectHandler.GetFigures[figureNames[n]];

                        ShaderHandler.UpdateShaderScripts();
                    }

                    // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                    if (is_selected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }

                }
                ImGui.EndCombo();
            }




            _selectedFigure ??= ObjectHandler.GetFigures.FirstOrDefault().Value;

            if (_selectedFigure?.HaveUI != null)
            {
                _selectedFigure.HaveUI.GetUI();
            }

            #endregion

            if (ImGui.BeginTabBar("Tabs"))
            {
                if (ImGui.BeginTabItem("Shader"))
                {
                    // phong lighting switch blinn light: switch
                    if (ImGui.Checkbox("Use Blinn lighting", ref UsingBlinnLight))
                    {
                        _selectedShader.SetUsingBlinn(UsingBlinnLight);
                        ShaderHandler.UpdateShaderScripts();
                    }

               


                    // rim light : Check box
                    if (ImGui.Checkbox("Use Rim Light", ref UsingRimLight))
                    {
                        _selectedShader.UsingRimLight(UsingRimLight);
                        ShaderHandler.UpdateShaderScripts();
                    }

                    ImGui.EndTabItem();
                }




                if (ImGui.BeginTabItem("Scene"))
                {
                    if (ImGui.Button("Set light color"))
                    {
                        _displayLightColorPicker = true;
                    }
                    if (_displayLightColorPicker) 
                        
                       {
                        SetColor(
                        "Set light color",
                        _selectedShader.SetLightColor,
                        ref _displayLightColorPicker,
                        ref _lightColor,
                        ref LightColorTK
                        );
                        ShaderHandler.UpdateShaderScripts();
                    }


                    if (windowUI != null)
                    {
                        windowUI.GetUI();
                    }

               


                    ImGui.EndTabItem();
                }




                #region MaterialTab
                if (ImGui.BeginTabItem("Material"))
                {
                    // selectable textuers tildeles også hvis den ikke allerede er sat
                    if (_selectedFigure == null) _selectedFigure = ObjectHandler.GetFigures.FirstOrDefault().Value;

                    figureNames = ObjectHandler.GetFigures.Keys.ToArray();
                    if (ImGui.BeginCombo("Figur", figureNames[_selectedFigureIndex]))
                    {
                        for (int n = 0; n < figureNames.Length; n++)
                        {
                            bool is_selected = _selectedFigureIndex == n;
                            if (ImGui.Selectable(figureNames[n], is_selected))
                            {
                                _selectedFigureIndex = n;
                                _selectedFigure = ObjectHandler.GetFigures[figureNames[n]];
                                ShaderHandler.UpdateShaderScripts();

                            }

                            // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                            if (is_selected)
                                ImGui.SetItemDefaultFocus();
                        }
                        ImGui.EndCombo();
                    }



                    if (ImGui.Button("Set color"))
                    {
                        _displayObjectColorPicker = true;
                    }
                    //if (_displayObjectColorPicker) SetColor(
                    //    "Set object color", 
                    //    SelectedShader.SetObjectColor, 
                    //    ref _displayObjectColorPicker, 
                    //    ref _objectColor,
                    //    ref ObjectColorTK);

                    //if (ImGui.Checkbox("useTexture", ref useTexture))
                    //{
                    //    SelectedShader.SetUsingTexture(useTexture);
                    //}

                    #region Albedo

                    ImGui.SeparatorText("Albedo");

                    // hvis der ikke er nogen textur oprettes en her
                    // bør nok flyttes til et andet sted der giver mere mening?
                    if (_selectedFigure.Material.Albedo == null) _selectedFigure.Material.Albedo = 
                            GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.CHECKERED);

                    if (ImGui.BeginCombo("Texture Selections", _selectedFigure.Material.Albedo.Name))
                    {
                        for (int n = 0; n < _selectableTextures.Length; n++)
                        {
                            bool is_selected = _currentAlbedoTextureIndex == n;
                            if (ImGui.Selectable(_selectableTextures[n], is_selected))
                            {
                                _currentAlbedoTextureIndex = n;
                                _selectedFigure.Material.Albedo = Texture.AllTextures[_selectableTextures[n]];
                            }

                            // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                            if (is_selected)
                                ImGui.SetItemDefaultFocus();
                        }
                        ImGui.EndCombo();
                    }

                    if (ImGui.BeginCombo("Filter Selections", _selectedFigure.Material.Albedo.FilterType.ToString()))
                    {
                        for (int n = 0; n < _filterTypes.Length; n++)
                        {
                            bool is_selected = _selectedAlbedoFilter == n;
                            if (ImGui.Selectable(_filterTypes[n], is_selected))
                            {
                                _selectedAlbedoFilter = n;
                                _selectedFigure.Material.Albedo.FilterType = (TextureFilterTypes)n;
                            }

                            // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                            if (is_selected)
                                ImGui.SetItemDefaultFocus();
                        }
                        ImGui.EndCombo();
                    }



                    if (ImGui.Checkbox("Albedo Anisotropic", ref _anisotropicAlbedo))
                    {
                        _selectedFigure.Material.Albedo.SetAnisotropic(_anisotropicAlbedo);
                    }

                    #endregion


                    #region lightMap

                    ImGui.SeparatorText("LightMap");





                    // hvis der ikke er nogen textur oprettes en her
                    // bør nok flyttes til et andet sted der giver mere mening?
                    if (_selectedFigure.Material.LightMap == null) _selectedFigure.Material.LightMap =
                            GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.CHECKERED);


                    if (ImGui.BeginCombo("Texture Selection", _selectedFigure.Material.LightMap.Name))
                    {
                        for (int n = 0; n < _selectableTextures.Length; n++)
                        {
                            bool is_selected = _currentLightMapTextureIndex == n;
                            if (ImGui.Selectable(_selectableTextures[n], is_selected))
                            {
                                _currentLightMapTextureIndex = n;
                                _selectedFigure.Material.LightMap = Texture.AllTextures[_selectableTextures[n]];
                            }
                        }
                        ImGui.EndCombo();
                    }


                    if (ImGui.BeginCombo("Filter Selection", _selectedFigure.Material.LightMap.FilterType.ToString()))
                    {
                        for (int n = 0; n < _filterTypes.Length; n++)
                        {
                            bool is_selected = _selectedLightMapFilter == n;
                            if (ImGui.Selectable(_filterTypes[n], is_selected))
                            {
                                _selectedLightMapFilter = n;
                                _selectedFigure.Material.LightMap.FilterType = (TextureFilterTypes)n;
                            }
                        }
                        ImGui.EndCombo();
                    }





                    if (ImGui.Checkbox("Light Anisotropic", ref _anisotropicLightMap))
                    {
                        _selectedFigure.Material.LightMap.SetAnisotropic(_anisotropicLightMap);
                    }

                    #endregion

                    #region SpecularMap

                    ImGui.SeparatorText("Specular map");

                    if (_selectedFigure.Material.SpecularMap == null) _selectedFigure.Material.SpecularMap =
                            GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.CHECKERED);

                    if (ImGui.BeginCombo("Specular Selection", _selectedFigure.Material.SpecularMap.Name))
                    {
                        for (int n = 0; n < _selectableTextures.Length; n++)
                        {
                            bool is_selected = _currentSpecularMapTextureIndex == n;
                            if (ImGui.Selectable(_selectableTextures[n], is_selected))
                            {
                                _currentSpecularMapTextureIndex = n;
                                _selectedFigure.Material.SpecularMap = Texture.AllTextures[_selectableTextures[n]];
                            }
                        }
                        ImGui.EndCombo();
                    }



                    if (ImGui.BeginCombo("Specular Filter", _selectedFigure.Material.SpecularMap.FilterType.ToString()))
                    {
                        for (int n = 0; n < _filterTypes.Length; n++)
                        {
                            bool is_selected = _selectedSpecularMapFilter == n;
                            if (ImGui.Selectable(_filterTypes[n], is_selected))
                            {
                                _selectedSpecularMapFilter = n;
                                _selectedFigure.Material.SpecularMap.FilterType = (TextureFilterTypes)n;
                            }
                        }
                        ImGui.EndCombo();
                    }



                    if (ImGui.Checkbox("Specular Anisotropic", ref _anisotropicSpeculatMap))
                    {
                        _selectedFigure.Material.SpecularMap.SetAnisotropic(_anisotropicSpeculatMap);
                    }

                    #endregion







                    #region Normal

                    ImGui.SeparatorText("Normal map");

                    if (_selectedFigure.Material.NormalTexture == null) _selectedFigure.Material.NormalTexture = 
                            GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.CHECKERED);

                    if (ImGui.BeginCombo("Normal Selection", _selectedFigure.Material.NormalTexture.Name))
                    {
                        for (int n = 0; n < _selectableTextures.Length; n++)
                        {
                            bool is_selected = _currentNormalTextureIndex == n;
                            if (ImGui.Selectable(_selectableTextures[n], is_selected))
                            {
                                _currentNormalTextureIndex = n;
                                _selectedFigure.Material.NormalTexture = Texture.AllTextures[_selectableTextures[n]];
                            }
                        }
                        ImGui.EndCombo();
                    }

                    
                    if (ImGui.SliderFloat("Texture intensity", ref normalStrength, 0, 1)) 
                    {
                        _selectedFigure.Material.NormalTexture.MapIntensity = normalStrength;
                    }

                    if (ImGui.BeginCombo("Normal Filter", _selectedFigure.Material.NormalTexture.FilterType.ToString()))
                    {
                        for (int n = 0; n < _filterTypes.Length; n++)
                        {
                            bool is_selected = _selectedNormalFilter == n;
                            if (ImGui.Selectable(_filterTypes[n], is_selected))
                            {
                                _selectedNormalFilter = n;
                                _selectedFigure.Material.NormalTexture.FilterType = (TextureFilterTypes)n;
                            }
                        }
                        ImGui.EndCombo();
                    }



                    if (ImGui.Checkbox("Normal Anisotropic", ref _anisotropicNormal))
                    {
                        _selectedFigure.Material.NormalTexture.SetAnisotropic(_anisotropicNormal);
                    }


                    #endregion
                    ImGui.EndTabItem();

                }



                if (ImGui.BeginTabItem("Shaders"))
                {
                    Shader[] shadersInFigure = _selectedFigure.Shaders;
                    string[] shaderNames = new string[shadersInFigure.Length];
                    int nameInt = 0;
                    foreach (Shader shad in shadersInFigure)
                    {
                        shaderNames[nameInt] = $"{nameInt++}";
                    }

                    if (_selectedShaderIndex > shadersInFigure.Length) _selectedShaderIndex = 0;

                    if (ImGui.BeginCombo("Shader", shaderNames[_selectedShaderIndex]))
                    {
                        for (int n = 0; n < shaderNames.Length; n++)
                        {
                            bool is_selected = _selectedShaderIndex == n;
                            if (ImGui.Selectable(shaderNames[n], is_selected))
                            {
                                _selectedShaderIndex = n;
                                _selectedShader = shadersInFigure[n];
                                ShaderHandler.UpdateShaderScripts();
                            }

                            // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                            if (is_selected)
                            {
                                ImGui.SetItemDefaultFocus();
                            }

                        }
                        ImGui.EndCombo();
                    }


                    ImGui.BeginTabBar("ShaderTabs");

                    if (ImGui.BeginTabItem("Vertex Shader"))
                    {




                        foreach (ShaderElementBase element in _selectedShader.ShaderScripts.Values)
                        {
                            if (element.ShaderType == ShaderType.VertexShader && element is IHaveUI fragmentShaderUI)
                            {
                                if (fragmentShaderUI.GetUI()) ShaderHandler.UpdateShaderScripts(); ;
                            }
                        }

                        ImGui.Text(_selectedShader.VertexShaderSource);
                        ImGui.EndTabItem();
                    }


                    if (ImGui.BeginTabItem("Geometry Shader"))
                    {
                        
                       if ( ImGui.Checkbox("Use Geometry shader", ref _selectedShader.UsesGeometryShader)) 
                            {
                            ShaderHandler.UpdateShaderScripts();
                            }

                        
                        if (_selectedShader.GetGeometryShaders().Count > 0)
                        {
                        uint[] geometryShaderNames = _selectedShader.GetGeometryShaders().Keys.ToArray();
                            if (_selectedGeoShaderIndex > _selectedShader.GetGeometryShaders().Count() -1)
                            {
                                _selectedGeoShaderIndex = (uint) 0;
                            }
                            _selectedGeometryShader = _selectedShader.GetGeometryShaders()[geometryShaderNames[_selectedGeoShaderIndex]];
                            if (ImGui.BeginCombo("Geometry shader", $"{geometryShaderNames[_selectedGeoShaderIndex]}"))
                            {
                                for (int n = 0; n < geometryShaderNames.Length; n++)
                                {
                                    bool is_selected = _selectedGeoShaderIndex == n;
                                    if (ImGui.Selectable($"{geometryShaderNames[n]}", is_selected))
                                    {
                                        _selectedGeoShaderIndex = (uint)n;
                                        _selectedGeometryShader = _selectedShader.GetGeometryShaders()[geometryShaderNames[n]];


                                        _selectedShader.SetActiveGeometryShader(geometryShaderNames[n]);
                                        ShaderHandler.UpdateShaderScripts();

                                    }




                                    // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                                    if (is_selected)
                                        ImGui.SetItemDefaultFocus();

                                }
                                ImGui.EndCombo();

                            }
                        }

        
         

                        if (_selectedGeometryShader is IHaveUI geoShaderSettings)
                        {
                            geoShaderSettings.GetUI();
                        }
           
                        ImGui.Text(_selectedShader.GeometryShaderSource);





                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Fragment Shader"))
                    {
                        foreach (ShaderElementBase element in _selectedShader.ShaderScripts.Values)
                        {
                            if (element.ShaderType == ShaderType.FragmentShader && element is IHaveUI fragmentShaderUI)
                            {
                                if (fragmentShaderUI.GetUI()) ShaderHandler.UpdateShaderScripts();
                            }
                        }


                        ImGui.Text(_selectedShader.FragmentShaderSource);
                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabItem();

                    ImGui.EndTabBar();
                }



                if (ImGui.BeginTabItem("Environment"))
                {
                    ImGui.Checkbox("use Environment map",ref UseEnvironment);
                 
                    ImGui.EndTabItem();
                }


                ImGui.EndTabBar();
            }

            #endregion

            
            ImGui.PopStyleColor(1);
            ImGui.End();


                if (_selectedFigure != null && _selectedFigure.GetType() == typeof(Objects.Plane))
                {
                    //Objects.Plane plane = (Objects.Plane)_selectedFigure;
                    //plane.Width = localPlanewidth;
                    //plane.Height = localPlaneheight;
                    //plane.SubdivideWidth = localPlanesubdivideWidth;
                    //plane.SubdivideHeight = localPlanesubdivideHeight;
                }
        }
            //ImGui.End();


        
        



        // Den her bruge både system og openTK vectors som indput, og har brug for begge.
        // system bruges til indputtet fra color picker, og openTK bruges til at læse værdier ind i TK
        private void SetColor(string windowTitle, ColorSet colorSet, ref bool opener, ref Sys.Vector3 colorContainer, ref TK.Vector3 TKColor)
        {
            ImGui.SetNextWindowSize(new Sys.Vector2(500, 500));
            ImGui.Begin(windowTitle, ref opener);
            if (ImGui.ColorPicker3("Pick color", ref colorContainer))
            {
                TKColor = new TK.Vector3(colorContainer.X, colorContainer.Y, colorContainer.Z);
                colorSet.Invoke(TKColor);
            }

            if (ImGui.Button("Luk")) opener = false;

            ImGui.End();
        }

        //private void GetDebugging()
        //{
        //    if (ImGui.Checkbox("testUV", ref displaUVTesting))
        //    {
        //        SelectedShader.SetUVTest(displaUVTesting);
        //        ImGui.Checkbox("DisplayTestingNormals", ref DisplayTestingNormals);
        //    }
        //}
    }
}
