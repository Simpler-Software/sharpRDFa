namespace sharpRDFa.RDF
{
    /*
     The subject denotes the resource, and the predicate denotes traits or aspects of the resource
     * and expresses a relationship between the subject and the object.
     * For example, one way to represent the notion "The sky has the color blue" in RDF is as the triple: 
     * a subject denoting "the sky", a predicate denoting "has the color", and an object denoting "blue". 
     */
    public class RDFTriple
    {
        public string Subject { get; set; }
        public string Predicate { get; set; }
        public ObjectNode Object { get; set; }

    }
}
