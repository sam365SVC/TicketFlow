package event

type TicketUploat struct {
	ZoneName  string `json:"ZoneName"`
	TicketUrl string `json:"Url"`
}

type ReservationConfirmedEvent struct {
	CustomerName  string         `json:"CustomerName"`
	CustomerPhone string         `json:"CustomerPhone"`
	EventName     string         `json:"EventName"`
	Location      string         `json:"Location"`
	Tickets       []TicketUploat `json:"Tickets"`
}
