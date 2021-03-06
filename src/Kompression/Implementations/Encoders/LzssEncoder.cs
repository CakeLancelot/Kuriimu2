﻿using System.Collections.Generic;
using System.IO;
using Kompression.Implementations.Encoders.Headerless;
using Kompression.Implementations.PriceCalculators;
using Kompression.PatternMatch.MatchFinders;
using Kontract.Kompression;
using Kontract.Kompression.Configuration;
using Kontract.Kompression.Model;
using Kontract.Kompression.Model.PatternMatch;

namespace Kompression.Implementations.Encoders
{
    public class LzssEncoder : ILzEncoder
    {
        private Lz10HeaderlessEncoder _encoder;

        public LzssEncoder()
        {
            _encoder = new Lz10HeaderlessEncoder();
        }

        public void Configure(IInternalMatchOptions matchOptions)
        {
            matchOptions.CalculatePricesWith(() => new LzssPriceCalculator())
                .FindWith((options, limits) => new HistoryMatchFinder(limits, options))
                .WithinLimitations(() => new FindLimitations(0x3, 0x12, 1, 0x1000));
        }

        public void Encode(Stream input, Stream output, IEnumerable<Match> matches)
        {
            var outputStartPos = output.Position;
            output.Position += 0x10;
            _encoder.Encode(input, output, matches);

            var outputPos = output.Position;
            output.Position = outputStartPos;
            output.Write(new byte[] { 0x53, 0x53, 0x5A, 0x4C }, 0, 4);
            output.Position += 8;
            var decompressedSizeBuffer = new[]
            {
                (byte)input.Length,
                (byte)((input.Length>>8)&0xFF),
                (byte)((input.Length>>16)&0xFF),
                (byte)((input.Length>>24)&0xFF),
            };
            output.Write(decompressedSizeBuffer, 0, 4);

            output.Position = outputPos;
        }

        public void Dispose()
        {
            _encoder = null;
        }
    }
}
