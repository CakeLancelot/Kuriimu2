﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kanvas;
using Kontract.Interfaces.FileSystem;
using Kontract.Interfaces.Plugins.State;
using Kontract.Kanvas;
using Kontract.Models.Context;
using Kontract.Models.Image;
using Kontract.Models.IO;

namespace plugin_bandai_namco.Images
{
    class MtexState : IImageState, ILoadFiles, ISaveFiles
    {
        private readonly MTEX _mtex;

        public EncodingDefinition EncodingDefinition { get; }
        public IList<IKanvasImage> Images { get; private set; }

        public bool ContentChanged => Images.Any(x => x.ImageInfo.ContentChanged);

        public MtexState()
        {
            _mtex = new MTEX();

            EncodingDefinition = MtexSupport.MtexFormats.ToColorDefinition();
        }

        public async Task Load(IFileSystem fileSystem, UPath filePath, LoadContext loadContext)
        {
            var fileStream = await fileSystem.OpenFileAsync(filePath);
            var img = _mtex.Load(fileStream);

            Images = new List<IKanvasImage> { new KanvasImage(EncodingDefinition, img) };
        }

        public Task Save(IFileSystem fileSystem, UPath savePath, SaveContext saveContext)
        {
            var output = fileSystem.OpenFile(savePath, FileMode.Create, FileAccess.Write);
            _mtex.Save(output, Images[0]);            

            return Task.CompletedTask;
        }
    }
}
