using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ParameterSetuper : MonoBehaviour
{
    [SerializeField] private ParameterRequest[] _parameterRequests;
    
    public ParameterRequest[] ParameterRequests => _parameterRequests;

//#if UNITY_EDITOR
//    [MenuItem("Edit/Find Parameters")]
//#endif
//    private static void FindParametrs()
//    {
//        string[] parametrsGUID = AssetDatabase.FindAssets("t:ParameterSO", new[] { "Assets/ScriptableObjects/Parameters" });
//        List<string> parametersPaths = new List<string>();
//        foreach (string GUID in parametrsGUID)
//            parametersPaths.Add(AssetDatabase.GUIDToAssetPath(GUID));
//        var parameters = new List<ParameterSO>();
//        foreach (string path in parametersPaths)
//            parameters.Add(AssetDatabase.LoadAssetAtPath(path, typeof(ParameterSO)) as ParameterSO);

//        var parameterRequests = Singltone.instance.GetParameterRequests();
//        foreach(var pReq in parameterRequests)
//        {
//            if (pReq.Active == false) continue;
//            string[] requestsGUID = AssetDatabase.FindAssets("t:" + pReq.Type, new[] { pReq.Path });
//            List<string> requestsPaths = new List<string>();
//            foreach (string GUID in requestsGUID)
//                requestsPaths.Add(AssetDatabase.GUIDToAssetPath(GUID));
//            Type type = Type.GetType(pReq.Type);
//            foreach(string path in requestsPaths)
//            {
//                var request = AssetDatabase.LoadAssetAtPath(path, type) as IRequiredParameters;
//                Debug.Log(request);
//                request.Setup(parameters);
//            }
//        }
//    }
}

[Serializable]
public class ParameterRequest
{
    [SerializeField] private bool _active;
    [SerializeField] private string _type;
    [SerializeField] private string _path;

    public bool Active => _active;
    public string Type => _type;
    public string Path => _path;
}

public interface IRequiredParameters
{
    void Setup(List<ParameterSO> parameters);

    //[SerializeField] private List<Parameter> _parameters;

    //public void Setup(List<ParameterSO> parameters)
    //{
    //    _parameters = new List<Parameter>();
    //    foreach (ParameterSO param in parameters)
    //        _parameters.Add(new Parameter(param.name, 0));
    //}
}

public class Singltone
{
    public static Singltone instance
    {
        get 
        {
            if (_instance == null)
                _instance = new Singltone();
            return _instance;
        }
    }

    private static Singltone _instance;

    public ParameterRequest[] GetParameterRequests()
    {
        var p = MonoBehaviour.FindObjectOfType<ParameterSetuper>();
        return p.ParameterRequests;
    }
}