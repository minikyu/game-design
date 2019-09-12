using UnityEngine;
using UnityEditor;
using System;

public class CustomShaderGUI : ShaderGUI
{
	MaterialEditor editor;
	MaterialProperty[] properties;
	Material target;
	enum SpecularChoice {True, False}
	enum ShaderTypeChoice {NORMAL_ONLY,TEX_AND_LIGHT }

	public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
	{
		this.editor = editor;
		this.properties = properties;
		this.target = editor.target as Material;
		
		//shader类型
		ShaderTypeChoice shaderTypeChoice = ShaderTypeChoice.TEX_AND_LIGHT;
		if (!target.IsKeywordEnabled("TEX_AND_LIGHT")) shaderTypeChoice = ShaderTypeChoice.NORMAL_ONLY;
		EditorGUI.BeginChangeCheck();
		shaderTypeChoice = (ShaderTypeChoice)EditorGUILayout.EnumPopup(
			new GUIContent("Shader Type"), shaderTypeChoice
		);

		if (EditorGUI.EndChangeCheck())
		{
			if (shaderTypeChoice == ShaderTypeChoice.NORMAL_ONLY)
			{
				target.DisableKeyword("TEX_AND_LIGHT");
			}
			else
			{
				target.EnableKeyword("TEX_AND_LIGHT");
			}
		}

		
		if (shaderTypeChoice == ShaderTypeChoice.TEX_AND_LIGHT)
		{
			//纹理
			MaterialProperty mainTex = FindProperty("_MainTex", properties);
			GUIContent mainTexLabel = new GUIContent(mainTex.displayName);
			editor.TextureProperty(mainTex, mainTexLabel.text);

			//主颜色
			MaterialProperty mainColor = FindProperty("_MainColor", properties);
			GUIContent mainColorLabel = new GUIContent(mainColor.displayName);
			editor.ColorProperty(mainColor, mainColorLabel.text);
			
			//镜面反射高光
			SpecularChoice specularChoice = SpecularChoice.False;
			if (target.IsKeywordEnabled("USE_SPECULAR")) specularChoice = SpecularChoice.True;
			EditorGUI.BeginChangeCheck();
			specularChoice = (SpecularChoice)EditorGUILayout.EnumPopup(
				new GUIContent("Use Specular?"), specularChoice
			);

			if (EditorGUI.EndChangeCheck())
			{
				if (specularChoice == SpecularChoice.True)
					target.EnableKeyword("USE_SPECULAR");
				else
					target.DisableKeyword("USE_SPECULAR");
			}

			if (specularChoice == SpecularChoice.True)
			{
				MaterialProperty shininess = FindProperty("_Shininess", properties);
				GUIContent shininessLabel = new GUIContent(shininess.displayName);
				editor.FloatProperty(shininess, "Specular Factor");

				MaterialProperty SpecularColor = FindProperty("_Specular", properties);
				GUIContent SpecularColorLabel = new GUIContent(SpecularColor.displayName);
				editor.ColorProperty(SpecularColor, SpecularColorLabel.text);
			}
		}


	}
}
