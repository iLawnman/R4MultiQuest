using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using UnityEngine;
using VContainer;

    public class ConfigDataContainer
    {
        public ApplicationData ApplicationData = new ApplicationData();
        [Inject] private readonly ApplicationSettings _applicationSettings;
    }