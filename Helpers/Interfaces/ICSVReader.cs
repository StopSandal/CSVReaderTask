﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    public interface ICSVReader
    {
        public Task ReadFileAndSaveToDBAsync(string filePath);
    }
}
