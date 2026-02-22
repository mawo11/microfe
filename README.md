# microfe


┌──────────────┐        HTTP       ┌─────────────────────┐
│  YARP Proxy  │ ────────────────▶ │  External Config API│
│  (.NET 8)    │                   │  (JSON routes)      │
└──────────────┘                   └─────────────────────┘
        │
        ▼
 Custom IProxyConfigProvider
        │
        ▼
  Dynamic reload (ChangeToken)
