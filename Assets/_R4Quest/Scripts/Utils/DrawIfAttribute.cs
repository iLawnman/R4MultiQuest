using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class DrawIfAttribute : PropertyAttribute
{
    public enum PanelTypeEnum
    {
        None,
        ImgTtleTxt,
        FourImg,
    }
    
    public string comparedPropertyName { get; private set; }
    public object comparedValue { get; private set; }
    public PanelTypeEnum panelType { get; private set; }
    
    public DrawIfAttribute(string comparedPropertyName, object comparedValue, PanelTypeEnum panelType = PanelTypeEnum.ImgTtleTxt)
    {
        this.comparedPropertyName = comparedPropertyName;
        this.comparedValue = comparedValue;
        this.panelType = panelType;
    }
}