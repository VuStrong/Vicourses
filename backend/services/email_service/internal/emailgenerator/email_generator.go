package emailgenerator

import (
	"fmt"

	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/models"
	"github.com/matcornic/hermes/v2"
)

type EmailGenerator struct {
	From       string
	To         string
	EmailType  string
	Payload    map[string]any
	AppName    string
	AppLogoUrl string
	WebUrl     string
}

func (g *EmailGenerator) Generate() (*models.EmailModel, error) {
	var html string
	var err error = nil
	var subject string

	switch g.EmailType {
	case "confirm_email":
		html, err = g.generateConfirmEmailHtml()
		subject = "Confirm your account"
	case "reset_password":
		html, err = g.generateResetPasswordHtml()
		subject = "Reset password"
	case "course_approved":
		html, err = g.generateCourseApprovedHtml()
		subject = "Course approved"
	case "course_not_approved":
		html, err = g.generateCourseNotApprovedHtml()
		subject = "Course not approved"
	default:
		return nil, fmt.Errorf("EmailType %s is invalid", g.EmailType)
	}

	if err != nil {
		return nil, err
	}

	emailModel := models.EmailModel{
		From:        g.From,
		To:          g.To,
		ContentType: "text/html",
		Body:        html,
		Subject:     subject,
	}

	return &emailModel, nil
}

// Confirm email is the email sent to users after they created an account
func (g *EmailGenerator) generateConfirmEmailHtml() (string, error) {
	h := hermes.Hermes{
		Product: hermes.Product{
			Name: g.AppName,
			Link: g.WebUrl,
			Logo: g.AppLogoUrl,
		},
	}

	h.Theme = &hermes.Default{}

	return h.GenerateHTML(hermes.Email{
		Body: hermes.Body{
			Name: g.Payload["userName"].(string),
			Intros: []string{
				fmt.Sprintf("Welcome to %s! We're very excited to have you on board.", g.AppName),
			},
			Actions: []hermes.Action{
				{
					Instructions: fmt.Sprintf("To get started with %s, please click here:", g.AppName),
					Button: hermes.Button{
						Text: "Confirm your account",
						Link: g.Payload["link"].(string),
					},
				},
			},
			Outros: []string{
				"Need help, or have questions? Just reply to this email, we'd love to help.",
			},
		},
	})
}

// Reset password email is the email sent to users when they request reset their account password
func (g *EmailGenerator) generateResetPasswordHtml() (string, error) {
	h := hermes.Hermes{
		Product: hermes.Product{
			Name: g.AppName,
			Link: g.WebUrl,
			Logo: g.AppLogoUrl,
		},
	}

	h.Theme = &hermes.Default{}

	return h.GenerateHTML(hermes.Email{
		Body: hermes.Body{
			Name: g.Payload["userName"].(string),
			Intros: []string{
				"You have received this email because a password reset request for Vicourses account was received.",
			},
			Actions: []hermes.Action{
				{
					Instructions: "Click the button below to reset your password:",
					Button: hermes.Button{
						Color: "#DC4D2F",
						Text:  "Reset your password",
						Link:  g.Payload["link"].(string),
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

// Course approved email is the email sent to users when one of their courses is approved
func (g *EmailGenerator) generateCourseApprovedHtml() (string, error) {
	h := hermes.Hermes{
		Product: hermes.Product{
			Name: g.AppName,
			Link: g.WebUrl,
			Logo: g.AppLogoUrl,
		},
	}

	h.Theme = &hermes.Default{}

	return h.GenerateHTML(hermes.Email{
		Body: hermes.Body{
			Name: g.Payload["userName"].(string),
			FreeMarkdown: hermes.Markdown(fmt.Sprintf(`
Your course has been approved: **%s**

Now anyone can see your course in our platform. We think your course will sell well (MAYBE)

---

`, g.Payload["courseName"].(string))),
			Outros: []string{
				"Need help, or have questions? Just reply to this email, we'd love to help.",
			},
		},
	})
}

// Course not approved email is the email sent to users when one of their courses is not approved
func (g *EmailGenerator) generateCourseNotApprovedHtml() (string, error) {
	h := hermes.Hermes{
		Product: hermes.Product{
			Name: g.AppName,
			Link: g.WebUrl,
			Logo: g.AppLogoUrl,
		},
	}

	h.Theme = &hermes.Default{}

	markdown := fmt.Sprintf(`
Your course has been declined by our team: **%s**

Reasons:

`, g.Payload["courseName"].(string))

	for _, reason := range g.Payload["reasons"].([]interface{}) {
		markdown += "- " + reason.(string) + "\n"
	}

	return h.GenerateHTML(hermes.Email{
		Body: hermes.Body{
			Name:         g.Payload["userName"].(string),
			FreeMarkdown: hermes.Markdown(markdown),
			Outros: []string{
				"Angry? Just reply to this email, we'd love to help.",
			},
		},
	})
}
