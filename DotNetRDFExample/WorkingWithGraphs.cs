using PlayWithDotNetRDF;
using System;
using System.Collections.Generic;
using System.Text;
using VDS.RDF;
using VDS.RDF.Parsing;

// https://github.com/dotnetrdf/dotnetrdf/wiki/UserGuide-Working-With-Graphs

namespace DotNetRDFExample
{
    public class WorkingWithGraphs
    {
        public void TestBaseUri()
        {
            Graph g = new Graph();
            g.BaseUri = new Uri("http://example.org/base");

            bool isEmpty = g.IsEmpty;
            INamespaceMapper namespaceMap = g.NamespaceMap;

            //Assuming we have some Graph g find all the URI Nodes
            foreach (IUriNode u in g.Nodes.UriNodes())
            {
                //Write the URI to the Console
                Console.WriteLine(u.Uri.ToString());
            }

            foreach (var triple in g.Triples)
            {
                Console.WriteLine(triple.Subject.ToString());
                Console.WriteLine(triple.Predicate.ToString());
                Console.WriteLine(triple.Object.ToString());
            }



        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IGraph CreateGraph()
        {
            // https://github.com/dotnetrdf/dotnetrdf/wiki/UserGuide-Using-The-Namespace-Mapper

            IGraph g = new Graph();
            g.BaseUri = new Uri("http://example.org/");

            //Define the Namespaces we want to use
            g.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            g.NamespaceMap.AddNamespace("ex", new Uri("http://example.org"));

            IUriNode dotNetRDF = g.CreateUriNode(UriFactory.Create("http://www.dotnetrdf.org"));
            IUriNode says = g.CreateUriNode(UriFactory.Create("http://example.org/says"));
            IUriNode description = g.CreateUriNode(UriFactory.Create("http://example.org/description"));
            IUriNode value = g.CreateUriNode(UriFactory.Create("http://example.org/value"));
            ILiteralNode helloWorld = g.CreateLiteralNode("Hello World");
            ILiteralNode bonjourMonde = g.CreateLiteralNode("Bonjour tout le Monde", "fr");


         

            //Define the same Triple as the previous example
            IUriNode exThis = g.CreateUriNode("ex:this");
            IUriNode rdfType = g.CreateUriNode("rdf:type");
            IUriNode exExample = g.CreateUriNode("ex:Example");
            g.Assert(new Triple(exThis, rdfType, exExample));


            g.Assert(new Triple(dotNetRDF, says, helloWorld));
            g.Assert(new Triple(dotNetRDF, says, bonjourMonde));

            IBlankNode blankNode = g.CreateBlankNode("myNodeID");
            ILiteralNode l = g.CreateLiteralNode("Some Text");
            ILiteralNode l2 = g.CreateLiteralNode("Some Text", "en");
            ILiteralNode l3 = g.CreateLiteralNode("1", new Uri(XmlSpecsHelper.XmlSchemaDataTypeInteger));

            g.Assert(new Triple(blankNode, description, l));
            g.Assert(new Triple(blankNode, description, l2));
            g.Assert(new Triple(blankNode, value, l3));


            foreach (Triple t in g.Triples)
            {
                Console.WriteLine(t.ToString());
            }

            return g;
        }

        public void TestGetNodes()
        {
            //Assuming we have some Graph g
            IGraph g = CreateGraph();

            //Selecting a Blank Node
            IBlankNode b = g.GetBlankNode("myNodeID");
            if (b != null)
            {
                Console.WriteLine("Blank Node with ID " + b.InternalID + " exists in the Graph");
            }
            else
            {
                Console.WriteLine("No Blank Node with the given ID existed in the Graph");
            }

            //Selecting Literal Nodes

            //Plain Literal with the given Value
            ILiteralNode l = g.GetLiteralNode("Some Text");

            //Literal with the given Value and Language Specifier
            ILiteralNode l2 = g.GetLiteralNode("Some Text", "en");

            //Literal with the given Value and DataType
            ILiteralNode l3 = g.GetLiteralNode("1", new Uri(XmlSpecsHelper.XmlSchemaDataTypeInteger));

            //Selecting URI Nodes

            //By URI
            IUriNode u = g.GetUriNode(new Uri("http://example.org/description"));

            //By Prefixed Name
            IUriNode u2 = g.GetUriNode("ex:value");

            // u3 is null since 
            IUriNode u3 = g.GetUriNode("ex:value2");
        }

        public void SelectingTriples()
        {
            //Assuming we have some Graph g
            IGraph g = CreateGraph();

            //Get all Triples involving a given Node
            IUriNode select = g.CreateUriNode(new Uri("http://example.org/select"));
            IEnumerable<Triple> ts = g.GetTriples(select);

            //Get all Triples which meet some criteria
            //Want to find everything that is rdf:type ex:Person
            IUriNode rdfType = g.CreateUriNode("rdf:type");
            IUriNode person = g.CreateUriNode("ex:Person");
            ts = g.GetTriplesWithPredicateObject(rdfType, person);

            //Get all Triples with a given Subject
            //We're reusing the node we created earlier
            ts = g.GetTriplesWithSubject(select);

            //Get all the Triples with a given Predicate
            ts = g.GetTriplesWithPredicate(rdfType);

            //Get all the Triples with a given Object
            ts = g.GetTriplesWithObject(person);
        }

        public IGraph CreateGraph1()
        {
            IGraph g = new Graph();
            g.BaseUri = new Uri("http://example1.org/");

            //Define the Namespaces we want to use
            g.NamespaceMap.AddNamespace("ex", new Uri("http://example1.org"));

            IUriNode philippe = g.CreateUriNode("ex:Philippe");
            IUriNode says = g.CreateUriNode(UriFactory.Create("http://example.org/says"));
            ILiteralNode helloWorld = g.CreateLiteralNode("Hello World");
            ILiteralNode bonjourMonde = g.CreateLiteralNode("Bonjour tout le Monde", "fr");

            g.Assert(new Triple(philippe, says, helloWorld));
            g.Assert(new Triple(philippe, says, bonjourMonde));

            return g;
        }

        public IGraph CreateGraph2()
        {
            IGraph g = new Graph();
            g.BaseUri = new Uri("http://example1.org/");

            //Define the Namespaces we want to use
            g.NamespaceMap.AddNamespace("ex", new Uri("http://example1.org"));

            IUriNode marina = g.CreateUriNode("ex:Marina");
            IUriNode says = g.CreateUriNode(UriFactory.Create("http://example.org/says"));
            ILiteralNode helloWorld = g.CreateLiteralNode("Hello World");
            ILiteralNode bonjourMonde = g.CreateLiteralNode("Привет мир", "ru");

            g.Assert(new Triple(marina, says, helloWorld));
            g.Assert(new Triple(marina, says, bonjourMonde));

            return g;
        }

        public void MergingGraphs()
        {
            IGraph g1 = CreateGraph1();
            IGraph g2 = CreateGraph2();

            g1.Merge(g2, true);

            foreach (Triple t in g1.Triples)
            {
                Console.WriteLine(t.ToString());
            }
        }


    }
}
