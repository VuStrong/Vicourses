package emails

type HtmlGenerator interface {
	GenerateHTML() (string, error)
}
