package lib

import (
	"github.com/matcornic/hermes/v2"
	"gopkg.in/gomail.v2"
)

func SendConfirmEmailLink(user User, link string) error {
	h := hermes.Hermes{
		Product: hermes.Product{
			Name: "Vicourses",
			Link: "https://strongtify.io.vn/",
			Logo: "https://res.cloudinary.com/dcnnikffw/image/upload/v1712564820/vicourses-high-resolution-logo_xqncow.png",
		},
	}

	h.Theme = &hermes.Default{}
	htmlBody, err := h.GenerateHTML(_generateConfirmLinkEmail(user, link))

	if err != nil {
		panic(err)
	}

	smtpConf := Conf.Smtp

	msg := gomail.NewMessage()
	msg.SetHeader("From", smtpConf.User)
	msg.SetHeader("To", user.Email)
	msg.SetHeader("Subject", "Confirm Email")
	msg.AddAlternative("text/html", htmlBody)

	d := gomail.NewDialer(smtpConf.Host, smtpConf.Port, smtpConf.User, smtpConf.Password)

	return d.DialAndSend(msg)
}

func SendResetPasswordLink(user User, link string) error {
	h := hermes.Hermes{
		Product: hermes.Product{
			Name: "Vicourses",
			Link: "https://strongtify.io.vn/",
			Logo: "https://res.cloudinary.com/dcnnikffw/image/upload/v1712564820/vicourses-high-resolution-logo_xqncow.png",
		},
	}

	h.Theme = &hermes.Default{}
	htmlBody, err := h.GenerateHTML(_generateResetPasswordEmail(user, link))

	if err != nil {
		panic(err)
	}

	smtpConf := Conf.Smtp

	msg := gomail.NewMessage()
	msg.SetHeader("From", smtpConf.User)
	msg.SetHeader("To", user.Email)
	msg.SetHeader("Subject", "Reset Password")
	msg.AddAlternative("text/html", htmlBody)

	d := gomail.NewDialer(smtpConf.Host, smtpConf.Port, smtpConf.User, smtpConf.Password)

	return d.DialAndSend(msg)
}

func _generateConfirmLinkEmail(user User, link string) hermes.Email {
	return hermes.Email{
		Body: hermes.Body{
			Name: user.Name,
			Intros: []string{
				"Welcome to Vicourses! We're very excited to have you on board.",
			},
			Actions: []hermes.Action{
				{
					Instructions: "To get started with Vicourses, please click here:",
					Button: hermes.Button{
						Text: "Confirm your account",
						Link: link,
					},
				},
			},
			Outros: []string{
				"Need help, or have questions? Just reply to this email, we'd love to help.",
			},
		},
	}
}

func _generateResetPasswordEmail(user User, link string) hermes.Email {
	return hermes.Email{
		Body: hermes.Body{
			Name: user.Name,
			Intros: []string{
				"You have received this email because a password reset request for Vicourses account was received.",
			},
			Actions: []hermes.Action{
				{
					Instructions: "Click the button below to reset your password:",
					Button: hermes.Button{
						Color: "#DC4D2F",
						Text:  "Reset your password",
						Link:  link,
					},
				},
			},
			Outros: []string{
				"If you did not request a password reset, no further action is required on your part.",
			},
			Signature: "Thanks",
		},
	}
}
