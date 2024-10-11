package emails

import (
	"fmt"

	"github.com/matcornic/hermes/v2"
)

type ConfirmEmail struct {
	BaseEmail
	UserName string
	Link     string
}

func (email *ConfirmEmail) GenerateHTML() (string, error) {
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
				fmt.Sprintf("Welcome to %s! We're very excited to have you on board.", email.AppName),
			},
			Actions: []hermes.Action{
				{
					Instructions: fmt.Sprintf("To get started with %s, please click here:", email.AppName),
					Button: hermes.Button{
						Text: "Confirm your account",
						Link: email.Link,
					},
				},
			},
			Outros: []string{
				"Need help, or have questions? Just reply to this email, we'd love to help.",
			},
		},
	})
}
