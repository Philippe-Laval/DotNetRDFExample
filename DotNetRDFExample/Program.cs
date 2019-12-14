using DotNetRDFExample;
using System;
using System.IO;
using VDS.RDF;
using VDS.RDF.Parsing;

using VDS.RDF.Query;
using VDS.RDF.Writing.Formatting;

namespace PlayWithDotNetRDF
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                PlayWithGraph p = new PlayWithGraph();
                p.PlayWithOverview();
                p.PlayWithHelloWorld();
                p.PlayWithReadingRdf();
                p.TesPlayWithWritingRdft();
                p.TestCompressingTurtleWriter();
                p.TestHtmlWriter();

                WorkingWithGraphs w = new WorkingWithGraphs();
                w.TestBaseUri();
                w.TestGetNodes();
                w.SelectingTriples();
                w.MergingGraphs();


                Test1();

                IGraph g = new Graph();

                TurtleParser ttlparser = new TurtleParser();

                //Load using a Filename
                ttlparser.Load(g, @"C:\Users\philippe\Documents\GitHub\DotNetRDFExample\DotNetRDFExample\Files\Test12.ttl");

                //First we need an instance of the SparqlQueryParser
                SparqlQueryParser parser = new SparqlQueryParser();

                //Then we can parse a SPARQL string into a query
                string query = File.ReadAllText(@"C:\Users\philippe\Documents\GitHub\DotNetRDFExample\DotNetRDFExample\Files\query1.txt");

                SparqlQuery q = parser.ParseFromString(query);

                SparqlResultSet o = g.ExecuteQuery(q) as SparqlResultSet;

                //Create a formatter
                INodeFormatter formatter = new TurtleFormatter();
                
                foreach (SparqlResult result in o)
                {
                    Console.WriteLine(result.ToString(formatter));
                }

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


        }

        public static void Test1()
        {
            //Create a Parameterized String
            SparqlParameterizedString queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("ex", new Uri("http://example.org/ns#"));

            //Set the SPARQL command
            //For more complex queries we can do this in multiple lines by using += on the
            //CommandText property
            //Note we can use @name style parameters here
            queryString.CommandText = "SELECT * WHERE { ?s ex:property @value }";

            //Inject a Value for the parameter
            queryString.SetUri("value", new Uri("http://example.org/value"));

            //When we call ToString() we get the full command text with namespaces appended as PREFIX
            //declarations and any parameters replaced with their declared values
            Console.WriteLine(queryString.ToString());

            //We can turn this into a query by parsing it as in our previous example
            SparqlQueryParser parser = new SparqlQueryParser();
            SparqlQuery query = parser.ParseFromString(queryString);
        }


    }
}
