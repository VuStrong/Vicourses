package models

type EmailModel struct {
	From        string `json:"from"`
	To          string `json:"to"`
	ContentType string `json:"contentType"`
	Subject     string `json:"subject"`
	Body        string `json:"body"`
}
