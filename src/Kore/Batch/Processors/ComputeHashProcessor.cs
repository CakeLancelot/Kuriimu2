﻿using System;
using System.IO;
using Kontract.Interfaces;
using Kontract.Interfaces.Plugins.State.Intermediate;
using Kontract.Models;

namespace Kore.Batch.Processors
{
    public class ComputeHashProcessor : IBatchHashProcessor
    {
        private readonly IHashAdapter _hashAdapter;

        public ComputeHashProcessor(IHashAdapter hashAdapter)
        {
            _hashAdapter = hashAdapter ?? throw new ArgumentNullException(nameof(hashAdapter));
        }

        public BatchHashResult Process(string inputFile, IKuriimuProgress progress)
        {
            var task = _hashAdapter.Compute(File.OpenRead(inputFile), progress);
            task.Wait();
            var result = task.Result;
            return new BatchHashResult(result.IsSuccessful, inputFile, result.Result);
        }
    }
}
