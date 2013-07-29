sharpRDFa
=========

The sharpRDFa is a .Net RDFa parser which can generates triples from HTML/XHTML documents. Results are represented in a form of Collection of RDF triples that can be used to store directly in a triple store. 

### Get Started

Sample Input File (HTML5_RDFa_1.1.html)

```html
<html prefix="dc: http://purl.org/dc/elements/1.1/ foaf: http://xmlns.com/foaf/0.1/" lang="en">
  <head>
    <title>John's Home Page</title>
    <link rel="profile" href="http://www.w3.org/1999/xhtml/vocab" />
    <base href="http://example.org/john-d/" />
    <meta property="dc:creator" content="Jonathan Doe" />
    <link rel="foaf:primaryTopic" href="http://example.org/john-d/#me" />
  </head>
  <body about="http://example.org/john-d/#me">
    <h1>John's Home Page</h1>
    <p>My name is <span property="foaf:nick">John D</span> and I like
      <a href="http://www.neubauten.org/" rel="foaf:interest"
        lang="de">Einstürzende Neubauten</a>.
    </p>
    <p>
      My <span rel="foaf:interest" resource="urn:ISBN:0752820907">favorite
      book is the inspiring <span about="urn:ISBN:0752820907"><cite
      property="dc:title">Weaving the Web</cite> by
      <span property="dc:creator">Tim Berners-Lee</span></span></span>.
    </p>
  </body>
</html>
```

Parse RDFa from a file

```csharp
var parser = new RDFaParser();
var triples = parser.ParseRDFTriplesFromFile("Resource\\HTML5_RDFa_1.1.html");
foreach (var rdfTriple in triples)
{
	Console.WriteLine(string.Format("<{0}> <{1}> <{2}>", rdfTriple.Subject, rdfTriple.Predicate, rdfTriple.Objecto));
}
```

Console output

```
<http://example.org/john-d/> <http://purl.org/dc/elements/1.1/creator> <Jonathan Doe>
<http://example.org/john-d/> <http://xmlns.com/foaf/0.1/primaryTopic> <http://example.org/john-d/#me>
<http://example.org/john-d/#me> <http://xmlns.com/foaf/0.1/nick> <John D>
<http://example.org/john-d/#me> <http://xmlns.com/foaf/0.1/interest> <http://www.neubauten.org/>
<http://example.org/john-d/#me> <http://xmlns.com/foaf/0.1/interest> <urn:ISBN:0752820907>
<urn:ISBN:0752820907> <http://purl.org/dc/elements/1.1/title> <Weaving the Web>
<urn:ISBN:0752820907> <http://purl.org/dc/elements/1.1/creator> <Tim Berners-Lee>
```

### Features
* Support Parse from file and parse from file
* Parser out put can be save in NTriples fromat and RDF xml format
* Extensive unit tests written using NUnit test framework


### Requirements
.Net 4.0 or newer

### Licensing
The sharpRDFa library and tests are licensed under the BSD-3-Clause license.

### Specifications
* Syntax and processing rules for embedding RDF through attributes - http://www.w3.org/TR/rdfa-core/
* Rich Structured Data Markup for Web Documents - http://www.w3.org/TR/rdfa-primer/
