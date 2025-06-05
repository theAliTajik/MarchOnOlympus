using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(AnimatorHelper), true)]
public class AnimatorHelperEditor : Editor
{
   private const string CONST_PREFIX = "private const string ";
   private const string ANIM_PREFIX = "ANIM_";
   private const string PARAM_PREFIX = "PARAM_";

   string m_generatedAnimationCode;
   string m_generatedParametersCode;

   private AnimatorHelper m_animatorHelper;
   
   void OnEnable()
   {
	   if (m_animatorHelper == null)
	   {
		   m_animatorHelper = (AnimatorHelper)target;
	   }
   }
   
   public override void OnInspectorGUI()
   {
        DrawDefaultInspector();
		UpdateGeneratedUiElements();
		
		//Show all the animation names in code format
		if (!string.IsNullOrEmpty(m_generatedAnimationCode))
		{
			EditorGUILayout.TextArea(m_generatedAnimationCode);
		}
		
		if (!string.IsNullOrEmpty(m_generatedParametersCode))
		{
			EditorGUILayout.Space();
			EditorGUILayout.TextArea(m_generatedParametersCode);
		} 
   }

   private void UpdateGeneratedUiElements()
   {
	   m_animatorHelper.GetAnimations();
	   ReloadPreviewInstances();
	   GenerateStateNamesCode();
	   GenerateParameterNameCode();
   }

   private void GenerateStateNamesCode()
   {
	   List<AnimatorHelper.AnimationHelperState> animationsList = m_animatorHelper.GetAnimationList();
	   m_generatedAnimationCode = "";
	   for (int i = 0; i < animationsList.Count; ++i)
	   {
		   string newLine = i < (animationsList.Count - 1) ? "\n" : String.Empty;
		   string animation = animationsList[i].animationName;
		   string constName = FormatNameForConst(animation);
		   m_generatedAnimationCode += CONST_PREFIX + ANIM_PREFIX + constName + " = \"" + animation + "\";" + newLine;
	   }
   }
    
   private void GenerateParameterNameCode()
   {
	   m_generatedParametersCode = "";

	   AnimatorControllerParameter[] parameters = m_animatorHelper.GetAnimatorParameters();
	   if (parameters == null)
	   {
		   return;
	   }

	   int numParameters = parameters.Length;
	   if (numParameters > 0)
	   {
		   for (int i = 0; i < numParameters; i++)
		   {
			   string typeString = "";
			   if (parameters[i].type == AnimatorControllerParameterType.Bool)
			   {
				   typeString = "_B";
			   }
			   else if (parameters[i].type == AnimatorControllerParameterType.Trigger)
			   {
				   typeString = "_T";
			   }
			   else if (parameters[i].type == AnimatorControllerParameterType.Float)
			   {
				   typeString = "_F";
			   }
			   else if (parameters[i].type == AnimatorControllerParameterType.Int)
			   {
				   typeString = "_I";
			   }
			   string newLine = i < (numParameters- 1) ? "\n" : String.Empty;

			   string paramName = parameters[i].name;
			   string constName = FormatNameForConst(paramName);
			   string generatedString = CONST_PREFIX + PARAM_PREFIX  + constName + typeString + " = \"" + paramName + "\";" + newLine;
			   m_generatedParametersCode += generatedString;
		   }
	   }
   }

   private string FormatNameForConst(string originalName)
   {
	   string constName = originalName;
	   constName = constName.ToUpper();
	   constName = constName.Replace(" ", "_");
	   constName = constName.Replace('-', '_');
	   return constName;
   }
}
