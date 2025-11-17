struct CopyOperation {
    public string Regexp;
    public string Target;
}

struct ReplaceOperation {
    public string Regexp;
    public string Value;
    public string[] Target;
}

struct BufferedCopyOp {
    public string Target;
    public string[] Regexps;
    public string[] Outputs;
}