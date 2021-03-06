﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace sharpRDFa.Tests
{
    [TestFixture]
    public class RDFWriterTests
    {
        [Test]
        public void WriteToXML_WithValidTripes_ExpectedFileCanBeFoundInExpectedLocation()
        {
            var parser = new RDFaParser();
            var writer = new RDFWriter();
            var triples = parser.ParseRDFTriplesFromFile("Resource\\alice-example.html");
            if (File.Exists("sample.rdf"))
                File.Delete("sample.rdf");
            writer.WriteToXML(triples, "out\\sample.rdf");
            Assert.IsTrue(File.Exists("out\\sample.rdf"));
        }
        
        [Test]
        public void WriteToNTriples_WithValidTripes_ExpectedFileCanBeFoundInExpectedLocation()
        {
            var parser = new RDFaParser();
            var writer = new RDFWriter();
            var triples = parser.ParseRDFTriplesFromFile("Resource\\alice-example.html");

            if (File.Exists("sample.nt"))
                File.Delete("sample.nt");
            writer.WriteToXML(triples, "out\\sample.nt");
            Assert.IsTrue(File.Exists("out\\sample.nt"));
        }
    }
}
