package emails

import (
	"github.com/matcornic/hermes/v2"
)

type ResetPasswordEmail struct {
	BaseEmail
	UserName string
	Link     string
}

func (email *ResetPasswordEmail) GenerateHTML() (string, error) {
	h := hermes.Hermes{
		Product: hermes.Product{
			Name: email.AppName,
			Link: email.WebUrl,
			Logo: email.AppLogoUrl,
		},
	}

	h.Theme = &hermes.Default{}

	return h.GenerateHTML(hermes.Email{
		Body: hermes.Body{
			Name: email.UserName,
			Intros: []string{
				"You have received this email because a password reset request for Vicourses account was received.",
			},
			Actions: []hermes.Action{
				{
					Instructions: "Click the button below to reset your password:",
					Button: hermes.Button{
						Color: "#DC4D2F",
						Text:  "Reset your password",
						Link:  email.Link,
					},
				},
			},
			Outros: []string{
				"If you did not request a password reset, no further action is required on your part.",
			},
			Signature: "Thanks",
		},
	})
}
