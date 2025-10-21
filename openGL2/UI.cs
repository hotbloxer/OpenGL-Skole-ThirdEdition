using ClickableTransparentOverlay;
using ImGuiNET;
using openGL2.Objects;
using openGL2.Shaders;
using openGL2.Textures;
using static openGL2.Textures.Texture;
using Sys = System.Numerics;
using TK = OpenTK.Mathematics;


namespace openGL2
{

    public class UI : Overlay
    {

        // UI input
        public static bool UsingBlinnLight = true; 
        public static bool UsingCellShading = false; 
        public static bool UsingRimLight = false;
        public static bool DisplayTestingNormals = false;
        public static bool displaUVTesting = false;
        public static bool useTexture = true;
        public static bool useChecker = false;

        // den eneste shader der er lige nu
        Shader SelectedShader = Shader.shaders[0];

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

        // de fleste texturer og andre ting afhænger af hvilken figur der er aktiveret
        int _selectedFigureIndex = 0;
        Figure _selectedFigure;

        
        protected override void Render()
        {

            string[] _selectableTextures = Texture.AllTextures.Keys.ToArray();

            //ImGui.ShowDemoWindow();

            ImGui.SetNextWindowSize(new Sys.Vector2(400, 800));
            ImGui.Begin("OpenTK View");


            if (ImGui.BeginTabBar("Tabs"))
            {
                if (ImGui.BeginTabItem("Debugging"))
                {
                    GetDebugging();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Shader"))
                {
                    // phong lighting switch blinn light: switch
                    if (ImGui.Checkbox("Use Blinn lighting", ref UsingBlinnLight))
                    {
                        SelectedShader.SetUsingBlinn(UsingBlinnLight);
                    }

                    // activate simple cell shading : Check box
                    if (ImGui.Checkbox("Use Cell Shading", ref UsingCellShading))
                    {
                        SelectedShader.UsingCellShader(UsingCellShading);
                    }

                    // rim light : Check box
                    if (ImGui.Checkbox("Use Rim Light", ref UsingRimLight))
                    {
                        SelectedShader.UsingRimLight(UsingRimLight);
                    }

                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Scene"))
                {
                    if (ImGui.Button("Set light color"))
                    {
                        _displayLightColorPicker = true;
                    }
                    if (_displayLightColorPicker) SetColor(
                        "Set light color",
                        SelectedShader.SetLightColor,
                        ref _displayLightColorPicker,
                        ref _lightColor,
                        ref LightColorTK
                        );

                    ImGui.EndTabItem();
                }
                #region MaterialTab
                if (ImGui.BeginTabItem("Material"))
                {
                    // selectable textuers tildeles også hvis den ikke allerede er sat
                    if (_selectedFigure == null) _selectedFigure = ObjectHandler.GetFigures.FirstOrDefault().Value;

                    string[] figureNames = ObjectHandler.GetFigures.Keys.ToArray();
                    if (ImGui.BeginCombo("Figur", figureNames[_selectedFigureIndex]))
                    {
                        for (int n = 0; n < figureNames.Length; n++)
                        {
                            bool is_selected = _selectedFigureIndex == n;
                            if (ImGui.Selectable(figureNames[n], is_selected))
                            {
                                _selectedFigureIndex = n;
                                _selectedFigure = ObjectHandler.GetFigures[figureNames[n]];


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
                    if (_displayObjectColorPicker) SetColor(
                        "Set object color", 
                        SelectedShader.SetObjectColor, 
                        ref _displayObjectColorPicker, 
                        ref _objectColor,
                        ref ObjectColorTK);

                    if (ImGui.Checkbox("useTexture", ref useTexture))
                    {
                        SelectedShader.SetUsingTexture(useTexture);
                    }

                    #region Albedo

                    ImGui.SeparatorText("Albedo");

                    // hvis der ikke er nogen textur oprettes en her
                    // bør nok flyttes til et andet sted der giver mere mening?
                    if (_selectedFigure.Albedo == null) _selectedFigure.Albedo = Texture.GetCheckered();

                    if (ImGui.BeginCombo("Texture Selections", _selectedFigure.Albedo.Name))
                    {
                        for (int n = 0; n < _selectableTextures.Length; n++)
                        {
                            bool is_selected = _currentAlbedoTextureIndex == n;
                            if (ImGui.Selectable(_selectableTextures[n], is_selected))
                            {
                                _currentAlbedoTextureIndex = n;
                                _selectedFigure.Albedo = Texture.AllTextures[_selectableTextures[n]];
                            }

                            // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                            if (is_selected)
                                ImGui.SetItemDefaultFocus();
                        }
                        ImGui.EndCombo();
                    }

                    if (ImGui.BeginCombo("Filter Selections", _selectedFigure.Albedo.FilterType.ToString()))
                    {
                        for (int n = 0; n < _filterTypes.Length; n++)
                        {
                            bool is_selected = _selectedAlbedoFilter == n;
                            if (ImGui.Selectable(_filterTypes[n], is_selected))
                            {
                                _selectedAlbedoFilter = n;
                                _selectedFigure.Albedo.FilterType = (TextureFilterTypes) n;
                            }

                            // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                            if (is_selected)
                                ImGui.SetItemDefaultFocus();
                        }
                        ImGui.EndCombo();
                    }



                    if (ImGui.Checkbox("Albedo Anisotropic", ref _anisotropicAlbedo))
                    {
                        _selectedFigure.Albedo.SetAnisotropic(_anisotropicAlbedo);
                    }

                    #endregion


                    #region lightMap

                    ImGui.SeparatorText("LightMap");





                    // hvis der ikke er nogen textur oprettes en her
                    // bør nok flyttes til et andet sted der giver mere mening?
                    if (_selectedFigure.LightMap == null) _selectedFigure.LightMap = Texture.GetCheckered();


                    if (ImGui.BeginCombo("Texture Selection", _selectedFigure.LightMap.Name))
                    {
                        for (int n = 0; n < _selectableTextures.Length; n++)
                        {
                            bool is_selected = _currentLightMapTextureIndex == n;
                            if (ImGui.Selectable(_selectableTextures[n], is_selected))
                            {
                                _currentLightMapTextureIndex = n;
                                _selectedFigure.LightMap = Texture.AllTextures[_selectableTextures[n]];
                            }
                        }
                        ImGui.EndCombo();
                    }


                    if (ImGui.BeginCombo("Filter Selection", _selectedFigure.LightMap.FilterType.ToString()))
                    {
                        for (int n = 0; n < _filterTypes.Length; n++)
                        {
                            bool is_selected = _selectedLightMapFilter == n;
                            if (ImGui.Selectable(_filterTypes[n], is_selected))
                            {
                                _selectedLightMapFilter = n;
                                _selectedFigure.LightMap.FilterType = (TextureFilterTypes)n;
                            }
                        }
                        ImGui.EndCombo();
                    }





                    if (ImGui.Checkbox("Light Anisotropic", ref _anisotropicLightMap))
                    {
                        _selectedFigure.LightMap.SetAnisotropic(_anisotropicLightMap);
                    }

                    #endregion

                    #region SpecularMap

                    ImGui.SeparatorText("Specular map");

                    if (_selectedFigure.SpecularMap == null) _selectedFigure.SpecularMap = Texture.GetCheckered();

                    if (ImGui.BeginCombo("Specular Selection", _selectedFigure.SpecularMap.Name))
                    {
                        for (int n = 0; n < _selectableTextures.Length; n++)
                        {
                            bool is_selected = _currentSpecularMapTextureIndex == n;
                            if (ImGui.Selectable(_selectableTextures[n], is_selected))
                            {
                                _currentSpecularMapTextureIndex = n;
                                _selectedFigure.SpecularMap = Texture.AllTextures[_selectableTextures[n]];
                            }
                        }
                        ImGui.EndCombo();
                    }



                    if (ImGui.BeginCombo("Specular Filter", _selectedFigure.SpecularMap.FilterType.ToString()))
                    {
                        for (int n = 0; n < _filterTypes.Length; n++)
                        {
                            bool is_selected = _selectedSpecularMapFilter == n;
                            if (ImGui.Selectable(_filterTypes[n], is_selected))
                            {
                                _selectedSpecularMapFilter = n;
                                _selectedFigure.SpecularMap.FilterType = (TextureFilterTypes)n;
                            }
                        }
                        ImGui.EndCombo();
                    }

               

                    if (ImGui.Checkbox("Specular Anisotropic", ref _anisotropicSpeculatMap))
                    {
                        _selectedFigure.LightMap.SetAnisotropic(_anisotropicSpeculatMap);
                    }

                    #endregion







                    #region NormalMap

                    ImGui.SeparatorText("Normal map");

                    if (_selectedFigure.NormalTexture == null) _selectedFigure.NormalTexture = Texture.GetCheckered();

                    if (ImGui.BeginCombo("Normal Selection", _selectedFigure.NormalTexture.Name))
                    {
                        for (int n = 0; n < _selectableTextures.Length; n++)
                        {
                            bool is_selected = _currentNormalTextureIndex == n;
                            if (ImGui.Selectable(_selectableTextures[n], is_selected))
                            {
                                _currentNormalTextureIndex = n;
                                _selectedFigure.NormalTexture = Texture.AllTextures[_selectableTextures[n]];
                            }
                        }
                        ImGui.EndCombo();
                    }



                    if (ImGui.BeginCombo("Normal Filter", _selectedFigure.NormalTexture.FilterType.ToString()))
                    {
                        for (int n = 0; n < _filterTypes.Length; n++)
                        {
                            bool is_selected = _selectedNormalFilter == n;
                            if (ImGui.Selectable(_filterTypes[n], is_selected))
                            {
                                _selectedNormalFilter = n;
                                _selectedFigure.NormalTexture.FilterType = (TextureFilterTypes)n;
                            }
                        }
                        ImGui.EndCombo();
                    }



                    if (ImGui.Checkbox("Normal Anisotropic", ref _anisotropicNormal))
                    {
                        _selectedFigure.LightMap.SetAnisotropic(_anisotropicNormal);
                    }


                    #endregion








                    ImGui.EndTabItem();

                }
                ImGui.EndTabBar();

                #endregion
            }
            ImGui.End();
        }



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

        private void GetDebugging()
        {
            if (ImGui.Checkbox("testUV", ref displaUVTesting))
            {
                SelectedShader.SetUVTest(displaUVTesting);
                ImGui.Checkbox("DisplayTestingNormals", ref DisplayTestingNormals);
            }
        }
    }
}
