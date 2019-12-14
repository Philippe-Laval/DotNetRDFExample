using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Parsing.Handlers;
using VDS.RDF.Writing;

namespace PlayWithDotNetRDF
{
    public class PlayWithGraph
    {
        /// <summary>
        /// https://github.com/dotnetrdf/dotnetrdf/wiki/UserGuide-Library-Overview
        /// </summary>
        public void PlayWithOverview()
        {
            /// Get a new Graph and set it's Base URI
            IGraph g = new Graph();
            g.BaseUri = new Uri("http://example.org/");

            #region URI Nodes

            // Create a URI Node that refers to the Base URI of the Graph
            // Only valid when the Graph has a non-null Base URI
            IUriNode thisGraph = g.CreateUriNode();

            // Create a URI Node that refers to some specific URI
            IUriNode dotNetRDF = g.CreateUriNode(UriFactory.Create("http://www.dotnetrdf.org"));

            // Create a URI Node using a Prefixed Name
            // Need to define a Namespace first
            g.NamespaceMap.AddNamespace("ex", UriFactory.Create("http://example.org/namespace/"));
            IUriNode pname = g.CreateUriNode("ex:demo");
            // Resulting URI is http://example.org/namespace/demo

            #endregion

            #region Blank Nodes

            //Create an anonymous Blank Node
            //Each call to this method generates a Blank Node with a new unique identifier within the Graph
            IBlankNode anon = g.CreateBlankNode();

            Console.WriteLine(anon.InternalID);

            //Create a named Blank Node
            //Reusing the same ID results in the same Blank Node within the Graph
            //Note that if the ID refers to an automatically assigned ID that is already in use the returned
            //Blank Node will be given an alternative ID
            IBlankNode named = g.CreateBlankNode("ID");

            Console.WriteLine(named.InternalID);

            #endregion

            #region Literal Nodes

            // Create a Plain Literal
            ILiteralNode plain = g.CreateLiteralNode("some value");

            //Create some Language Specified Literal
            ILiteralNode hello = g.CreateLiteralNode("hello", "en");
            ILiteralNode bonjour = g.CreateLiteralNode("bonjour", "fr");

            //Create some typed Literals
            //You'll need to be using the VDS.RDF.Parsing namespace to reference the constants used here
            ILiteralNode oneLiteral = g.CreateLiteralNode("1", UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInteger));
            ILiteralNode trueLiteral = g.CreateLiteralNode("true", UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeBoolean));

            #endregion

            #region Triples

            //Create some Nodes
            //IUriNode dotNetRDF = g.CreateUriNode(UriFactory.Create("http://www.dotnetrdf.org"));
            IUriNode createdBy = g.CreateUriNode(UriFactory.Create("http://example.org/createdBy"));
            ILiteralNode robVesse = g.CreateLiteralNode("Rob Vesse");

            // Assert this Triple
            Triple t1 = new Triple(dotNetRDF, createdBy, robVesse);
            g.Assert(t1);

            // Retract
            // g.Retract(t1);

            #endregion


            //Loop through Triples
            foreach (Triple t in g.Triples)
            {
                Console.WriteLine(t.ToString());
            }



        }

        /// <summary>
        /// https://github.com/dotnetrdf/dotnetrdf/wiki/UserGuide-Hello-World
        /// </summary>
        public void PlayWithHelloWorld()
        {
            IGraph g = CreateGraph();

            // Console.ReadLine();

            NTriplesWriter ntwriter = new NTriplesWriter();
            ntwriter.Save(g, "HelloWorld.nt");

            RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
            rdfxmlwriter.Save(g, "HelloWorld.rdf");

            CompressingTurtleWriter turtleWriter = new CompressingTurtleWriter();
            turtleWriter.Save(g, "HelloWorld.ttl");
        }

        public static IGraph CreateGraph()
        {
            IGraph g = new Graph();
            g.BaseUri = new Uri("http://example.org/");

            IUriNode dotNetRDF = g.CreateUriNode(UriFactory.Create("http://www.dotnetrdf.org"));
            IUriNode says = g.CreateUriNode(UriFactory.Create("http://example.org/says"));
            ILiteralNode helloWorld = g.CreateLiteralNode("Hello World");
            ILiteralNode bonjourMonde = g.CreateLiteralNode("Bonjour tout le Monde", "fr");

            g.Assert(new Triple(dotNetRDF, says, helloWorld));
            g.Assert(new Triple(dotNetRDF, says, bonjourMonde));

            foreach (Triple t in g.Triples)
            {
                Console.WriteLine(t.ToString());
            }

            return g;
        }

        /// <summary>
        /// https://github.com/dotnetrdf/dotnetrdf/wiki/UserGuide-Reading-RDF
        /// </summary>
        public void PlayWithReadingRdf()
        {
            #region Reading RDF from Files
            IGraph g = new Graph();
            IGraph h = new Graph();
            TurtleParser ttlparser = new TurtleParser();

            // Load using a Filename
            ttlparser.Load(g, "HelloWorld.ttl");

            // Load using a StreamReader
            ttlparser.Load(h, new StreamReader("HelloWorld.ttl"));

            try
            {
                IGraph g2 = new Graph();
                NTriplesParser ntparser = new NTriplesParser();

                //Load using Filename
                ntparser.Load(g2, "HelloWorld.nt");
            }
            catch (RdfParseException parseEx)
            {
                //This indicates a parser error e.g unexpected character, premature end of input, invalid syntax etc.
                Console.WriteLine("Parser Error");
                Console.WriteLine(parseEx.Message);
            }
            catch (RdfException rdfEx)
            {
                //This represents a RDF error e.g. illegal triple for the given syntax, undefined namespace
                Console.WriteLine("RDF Error");
                Console.WriteLine(rdfEx.Message);
            }
            #endregion

            #region Reading RDF from URIs

            try
            {
                IGraph g3 = new Graph();
                //UriLoader.Load(g3, new Uri("http://dbpedia.org/resource/Barack_Obama"));
            }
            catch (RdfParseException parseEx)
            {
                //This indicates a parser error e.g unexpected character, premature end of input, invalid syntax etc.
                Console.WriteLine("Parser Error");
                Console.WriteLine(parseEx.Message);
            }
            catch (RdfException rdfEx)
            {
                //This represents a RDF error e.g. illegal triple for the given syntax, undefined namespace
                Console.WriteLine("RDF Error");
                Console.WriteLine(rdfEx.Message);
            }
            #endregion

            #region Reading RDF from Embedded Resources

            try
            {
                IGraph g4 = new Graph();
                EmbeddedResourceLoader.Load(g4, "embedded.ttl, PlayWithDotNetRDF");
                Console.WriteLine(g4.IsEmpty);
            }
            catch (RdfParseException parseEx)
            {
                //This indicates a parser error e.g unexpected character, premature end of input, invalid syntax etc.
                Console.WriteLine("Parser Error");
                Console.WriteLine(parseEx.Message);
            }
            catch (RdfException rdfEx)
            {
                //This represents a RDF error e.g. illegal triple for the given syntax, undefined namespace
                Console.WriteLine("RDF Error");
                Console.WriteLine(rdfEx.Message);
            }
            #endregion

            #region Reading RDF from Strings

            Graph g5 = new Graph();
            StringParser.Parse(g5, "<http://example.org/a> <http://example.org/b> <http://example.org/c>.");

            Graph g6 = new Graph();
            NTriplesParser parser = new NTriplesParser();
            parser.Load(g6, new StringReader("<http://example.org/a> <http://example.org/b> <http://example.org/c>."));

            #endregion

            #region Store Parsers

            /*
            TripleStore store = new TripleStore();
            TriGParser trigparser = new TriGParser();

            //Load the Store
            trigparser.Load(store, "Example.trig");
            */

            #endregion

            #region Advanced Parsing

            // Create a Handler and use it for parsing
            CountHandler handler = new CountHandler();
            TurtleParser turtleParser = new TurtleParser();
            turtleParser.Load(handler, "HelloWorld.ttl");

            //Print the resulting count
            Console.WriteLine(handler.Count + " Triple(s)");

            // https://github.com/dotnetrdf/dotnetrdf/wiki/UserGuide-Handlers-API
            #endregion

            /*
Parser Class 	    Supported Input
NTriplesParser 	    NTriples
Notation3Parser 	Notation 3, Turtle, NTriples, some forms of TriG
NQuadsParser 	    NQuads, NTriples
RdfAParser 	        RDFa 1.0 embedded in (X)HTML, some RDFa 1.1 support
RdfJsonParser 	    RDF/JSON (Talis specification)
RdfXmlParser 	    RDF/XML
TriGParser 	        TriG
TriXParser 	        TriX
TurtleParser 	    Turtle, NTriples
JsonLdParser 	    JSON-LD             
             */

        }

        /// <summary>
        /// https://github.com/dotnetrdf/dotnetrdf/wiki/UserGuide-Writing-RDF
        /// </summary>
        public void TesPlayWithWritingRdft()
        {
            #region Basic Usage

            IGraph g = CreateGraph();

            //Assume that the Graph to be saved has already been loaded into a variable g
            RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();

            //Save to a File
            rdfxmlwriter.Save(g, "Example.rdf");

            //Save to a Stream
            rdfxmlwriter.Save(g, Console.Out);

            #region Writing to Strings

            //Assume that the Graph to be saved has already been loaded into a variable g
            //RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();

            String data1 = VDS.RDF.Writing.StringWriter.Write(g, rdfxmlwriter);

            // or

            //Assume that the Graph to be saved has already been loaded into a variable g
            //RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
            System.IO.StringWriter sw = new System.IO.StringWriter();

            //Call the Save() method to write to the StringWriter
            rdfxmlwriter.Save(g, sw);

            //We can now retrieve the written RDF by using the ToString() method of the StringWriter
            String data2 = sw.ToString();

            #endregion


            #endregion
        }

        public void TestCompressingTurtleWriter()
        {
            IGraph g = CreateGraph();

            CompressingTurtleWriter compressingTurtleWriter = new CompressingTurtleWriter();

            SaveGraph(g, compressingTurtleWriter, "Example.ttl");
        }

        public void TestHtmlWriter()
        {
            IGraph g = CreateGraph();

            HtmlWriter htmlWriter = new HtmlWriter();

            SaveGraph(g, htmlWriter, "Example.html");
        }


        public void SaveGraph(IGraph g, IRdfWriter writer, String filename)
        {
            //Set Pretty Print Mode on if supported
            if (writer is IPrettyPrintingWriter)
            {
                ((IPrettyPrintingWriter)writer).PrettyPrintMode = true;
            }

            //Set High Speed Mode forbidden if supported
            if (writer is IHighSpeedWriter)
            {
                ((IHighSpeedWriter)writer).HighSpeedModePermitted = false;
            }

            //Set Compression Level to High if supported
            if (writer is ICompressingWriter)
            {
                ((ICompressingWriter)writer).CompressionLevel = WriterCompressionLevel.High;
            }

            //Save the Graph
            writer.Save(g, filename);
        }


    }
}
