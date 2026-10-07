#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace AntistarAssets
{
    public class AntistarTooltip : EditorWindow
    {
        private const float WinWidthPct = 0.34f;
        private const float WinHeightPct = 0.88f;
        private const float WinWidthMin = 760f;
        private const float WinWidthMax = 1200f;
        private const float WinHeightMin = 520f;
        private const float WinHeightMax = 1040f;
        private const float Pad = 18f;

        private const string UrlPortal = "https://portal.antistar.gg/";
        private const string UrlDiscord = "https://discord.gg/antistar";
        private const string UrlWebsite = "https://antistar.gg";
        private const string UrlTwitter = "https://x.com/TeamAntistar";
        private const string UrlGumroad = "https://antistarstore.gumroad.com/";
        private const string UrlJinxxy = "https://jinxxy.com/Antistar";

        private const string PrefDismissed = "AntistarTooltipV2_Dismissed";
        private const string SessionShown = "AntistarTooltipV2_Shown";

        private static readonly Color ColBg = new Color(0.051f, 0.047f, 0.039f);
        private static readonly Color ColSurface = new Color(0.078f, 0.075f, 0.059f);
        private static readonly Color ColSurface2 = new Color(0.106f, 0.102f, 0.082f);
        private static readonly Color ColLine = new Color(0.165f, 0.157f, 0.125f);
        private static readonly Color ColText = new Color(0.941f, 0.937f, 0.914f);
        private static readonly Color ColMuted = new Color(0.659f, 0.651f, 0.612f);
        private static readonly Color ColDim = new Color(0.435f, 0.427f, 0.392f);
        private static readonly Color ColYellow = new Color(1f, 0.839f, 0f);
        private static readonly Color ColGreen = new Color(0.267f, 1f, 0.533f);
        private static readonly Color ColRedSoft = new Color(0.85f, 0.55f, 0.55f);
        private static readonly Color ColJinxxy = new Color(0.635f, 0.42f, 1f);
        private static readonly Color ColJinxxyBg = new Color(0.094f, 0.075f, 0.145f);
        private static readonly Color ColGumroad = new Color(1f, 0.565f, 0.91f);
        private static readonly Color ColGumroadBg = new Color(0.137f, 0.075f, 0.118f);

        private Texture2D _banner;
        private Vector2 _scroll;
        private readonly bool[] _faqOpen = new bool[10];

        private GUIStyle _styleWordmark;
        private GUIStyle _styleKicker;
        private GUIStyle _styleHeader;
        private GUIStyle _styleSub;
        private GUIStyle _styleBody;
        private GUIStyle _styleFaqTitle;
        private GUIStyle _styleFaqBody;
        private GUIStyle _styleBtnPrimary;
        private GUIStyle _styleBtnGhost;
        private GUIStyle _styleFooter;
        private bool _stylesBuilt;

        [MenuItem("Antistar Assets/Support & FAQ")]
        public static void ShowWindow()
        {
            Open();
        }

        [MenuItem("Antistar Assets/Show Popup Again")]
        public static void ShowPopupAgain()
        {
            EditorPrefs.DeleteKey(PrefDismissed);
            Open();
        }

        private static void Open()
        {
            var win = GetWindow<AntistarTooltip>(true, "Antistar", true);
            float w = Mathf.Clamp(Screen.currentResolution.width * WinWidthPct, WinWidthMin, WinWidthMax);
            float h = Mathf.Clamp(Screen.currentResolution.height * WinHeightPct, WinHeightMin, WinHeightMax);
            float x = (Screen.currentResolution.width - w) * 0.5f;
            float y = (Screen.currentResolution.height - h) * 0.5f;
            win.minSize = new Vector2(WinWidthMin, WinHeightMin);
            win.maxSize = new Vector2(WinWidthMax, WinHeightMax);
            win.position = new Rect(x, y, w, h);
            win.Show();
        }

        [InitializeOnLoadMethod]
        private static void AutoOpen()
        {
            if (EditorPrefs.GetBool(PrefDismissed, false)) return;
            if (SessionState.GetBool(SessionShown, false)) return;
            SessionState.SetBool(SessionShown, true);
            EditorApplication.delayCall += Open;
        }

        private void OnEnable()
        {
            _banner = Resources.Load<Texture2D>("AntistarBanner");
            if (_banner == null)
            {
                string[] guids = AssetDatabase.FindAssets("AntistarBanner t:Texture2D");
                if (guids.Length > 0)
                    _banner = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }
        }

        private void BuildStyles()
        {
            if (_stylesBuilt) return;
            _stylesBuilt = true;

            _styleWordmark = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 26,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = ColYellow }
            };

            _styleKicker = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 11,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = ColYellow }
            };

            _styleHeader = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 17,
                normal = { textColor = ColText }
            };

            _styleSub = new GUIStyle(EditorStyles.label)
            {
                fontSize = 12,
                normal = { textColor = ColDim }
            };

            _styleBody = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                fontSize = 13,
                wordWrap = true,
                richText = true,
                normal = { textColor = ColMuted }
            };

            _styleFaqTitle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 13,
                normal = { textColor = ColText }
            };

            _styleFaqBody = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                fontSize = 13,
                wordWrap = true,
                richText = true,
                normal = { textColor = ColMuted },
                padding = new RectOffset(8, 8, 8, 8)
            };

            _styleBtnPrimary = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.black },
                hover = { textColor = Color.black },
                active = { textColor = Color.black }
            };

            _styleBtnGhost = new GUIStyle(GUI.skin.button)
            {
                fontSize = 13,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = ColText }
            };

            _styleFooter = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = ColDim }
            };
        }

        private void OnGUI()
        {
            BuildStyles();
            DrawRect(new Rect(0, 0, position.width, position.height), ColBg);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            DrawBanner();
            DrawHero();
            DrawPortal();
            DrawSupport();
            DrawFaq();
            DrawReview();
            DrawPiracy();
            DrawDismiss();
            DrawFooter();

            EditorGUILayout.EndScrollView();
        }

        private void DrawBanner()
        {
            float w = position.width;
            float h = Mathf.Clamp(_banner != null && _banner.width > 0 ? w * ((float)_banner.height / _banner.width) : w / 4f, 90f, 230f);
            Rect r = GUILayoutUtility.GetRect(w, h);
            DrawRect(r, ColSurface);
            if (_banner != null)
            {
                GUI.DrawTexture(r, _banner, ScaleMode.StretchToFill);
            }
            else
            {
                GUI.Label(new Rect(r.x + Pad, r.y, r.width - Pad * 2, r.height), "ANTISTAR ★", _styleWordmark);
            }
            Separator();
        }

        private void DrawHero()
        {
            GUILayout.Space(14);
            Pane(() =>
            {
                GUILayout.Label("★ ANTISTAR · UGC, ASSET & GAMES STUDIO", _styleKicker);
                GUILayout.Space(4);
                GUILayout.Label("Thank you for the support.", _styleHeader);
                GUILayout.Space(2);
                GUILayout.Label("Modeled, textured, and rigged in-house by our team.", _styleBody);
            });
            Separator();
        }

        private void DrawPortal()
        {
            Section("★ YOUR FILES & THE CUSTOMER PORTAL", () =>
            {
                GUILayout.Label("Where your files live depends on the store you bought from:", _styleBody);
                GUILayout.Space(8);
                StoreBox("JINXXY", ColJinxxy, ColJinxxyBg,
                    "Everything you paid for is delivered right in your Jinxxy library. The portal is purely a free bonus on top.");
                GUILayout.Space(6);
                StoreBox("GUMROAD", ColGumroad, ColGumroadBg,
                    "Your purchase delivers a license key, and your downloads unlock on the portal once you register it. Older packages are the exception since their files sit directly in your Gumroad library.");
                GUILayout.Space(10);
                GUILayout.Label("On top of your purchase, the portal adds for free:", _styleBody);
                GUILayout.Space(6);
                Bullet("Free extras and .spp source files, when available");
                Bullet("Reference sheets and bonus clothing, when available");
                Bullet("Every download and free update in one library");
                Bullet("Extra giveaway entries and a customer role on Discord");
                GUILayout.Space(10);
                Rect whyBox = EditorGUILayout.BeginVertical();
                DrawRect(whyBox, new Color(0.157f, 0.137f, 0.043f));
                DrawRect(new Rect(whyBox.x, whyBox.y, 3f, whyBox.height), ColYellow);
                GUILayout.Space(8);
                Inset(() =>
                {
                    GUILayout.Label("★ WHY ARE THE EXTRAS NOT INSIDE THE PACKAGE?", _styleKicker);
                    GUILayout.Space(4);
                    GUILayout.Label("Some of them carry licenses that do not allow bundling with a paid product, but do allow free standalone distribution, so those live on the portal for verified customers. Others are simply our thank-you for verifying your license.", _styleBody);
                });
                GUILayout.Space(8);
                EditorGUILayout.EndVertical();
                GUILayout.Space(6);
                GUILayout.Label("Register once with the license key from your receipt or your store library page. Access does not expire.", _styleBody);
                GUILayout.Space(10);
                PrimaryButton("Open the Customer Portal", UrlPortal, 40f);
            });
        }

        private void DrawSupport()
        {
            Section("★ SUPPORT", () =>
            {
                GUILayout.Label("Stuck, curious, or something looks wrong? Open a ticket on the portal or in our Discord.", _styleBody);
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GhostButton("Join the Discord", UrlDiscord, 34f);
                GUILayout.Space(6);
                GhostButton("Website", UrlWebsite, 34f);
                GUILayout.Space(6);
                GhostButton("Follow us on X", UrlTwitter, 34f);
                GUILayout.EndHorizontal();
            });
        }

        private void DrawFaq()
        {
            Section("★ FAQ & TROUBLESHOOTING", () =>
            {
                int i = 0;
                Faq(ref i, "Textures are pink or missing",
                    "Install <b>Poiyomi Toon Shader</b> and keep it up to date. After installing, re-lock all materials and re-import the prefab. Pink always means the shader is missing, not that the asset is broken.");

                Faq(ref i, "VRCFury errors on import, or nothing appears on the avatar",
                    "Install <b>VRCFury</b> and <b>Poiyomi</b> before importing this package, then drag the prefab onto your avatar. Most of the magic only shows up in-game or in Gesture Manager, not in the editor scene, so upload before judging.");

                Faq(ref i, "A toggle is missing from my radial menu",
                    "Open the toggle settings on the prefab and check every entry points at the right object. A renamed or moved object breaks its toggle. If it looks right and still fails, re-import the prefab fresh and apply your changes again.");

                Faq(ref i, "Clothing clips through the avatar",
                    "Small clipping on extreme poses is normal for every rigged garment. Constant clipping in idle poses is not: that usually needs a fix from our team, so open a ticket with a screenshot and your avatar base. Also worth checking: body blendshapes (breast or muscle sliders) often need the matching blendshape on the clothing, listed on the product page.");

                Faq(ref i, "The base avatar's own hair or clothes will not hide with the outfit toggle",
                    "Expected. VRCFury toggles that reach outside the outfit's own prefab tend to break on upload, so our packages only toggle their own pieces. Hide the base avatar's items with your avatar's own toggles, or delete those meshes if you never use them.");

                Faq(ref i, "The color sliders do nothing",
                    "If you swapped materials, unlocked them, or something just went wrong, unlock and re-lock everything (Poiyomi, Lock All) and check again in-game. The editor preview does not always show what the radial sliders do.");

                Faq(ref i, "Is this everything? The store page showed more",
                    "Bought on Jinxxy? Everything you paid for is in your Jinxxy library. Bought on Gumroad? Newer products deliver a license key and the files unlock on the portal; older packages keep their files right in your Gumroad library. Anything listed as a bonus is a free optional extra on the portal, because some extras have licenses that allow free standalone distribution but not bundling with a paid product.");

                Faq(ref i, "An update came out. How do I get it, and will it break my edits?",
                    "Updates are free forever, in your portal library or on Jinxxy. Made your own edits? Duplicate your edited materials and prefabs before importing; a fresh import only touches our original files, never your copies.");

                Faq(ref i, "Which avatar bases does this fit?",
                    "Each edition is fitted for the base in its product name. Other bases have their own editions on the store, and community refits arrive through the Refit Program. Wearing it on an unsupported base means refitting it yourself.");

                Faq(ref i, "Can I edit it, retexture it, or use it commercially?",
                    "Editing and retexturing for yourself: always fine. Streaming and videos as yourself: always fine, monetized included. Selling anything built on it needs the commercial terms, written in plain English at licensing.antistar.gg.");

                GUILayout.Space(8);
                PrimaryButton("Still stuck? Open a ticket", UrlPortal, 36f);
            });
        }

        private void DrawReview()
        {
            Section("★ LEAVE A REVIEW", () =>
            {
                Rect box = EditorGUILayout.BeginVertical();
                DrawRect(box, ColSurface2);
                DrawRect(new Rect(box.x, box.y, 3f, box.height), ColYellow);
                GUILayout.Space(8);
                Inset(() =>
                {
                    var stars = new GUIStyle(EditorStyles.label) { fontSize = 18, normal = { textColor = ColYellow } };
                    GUILayout.Label("★★★★★", stars);
                    GUILayout.Space(4);
                    GUILayout.Label("Enjoying it? A review means the world to a studio our size, and we read every single one.", _styleBody);
                });
                GUILayout.Space(10);
                EditorGUILayout.EndVertical();
            });
        }

        private void DrawPiracy()
        {
            Section("★ A NOTE ON PIRACY", () =>
            {
                Rect box = EditorGUILayout.BeginVertical();
                DrawRect(box, new Color(0.12f, 0.075f, 0.075f));
                GUILayout.Space(8);
                Inset(() =>
                {
                    var red = new GUIStyle(_styleBody) { normal = { textColor = ColRedSoft } };
                    GUILayout.Label("Using a pirated copy? Please consider buying it.", red);
                    GUILayout.Space(4);
                    GUILayout.Label("Gumroad has heavily discounted regional pricing. Antistar is a small studio with workers and bills, and most of every sale goes straight to the people who made this.", _styleBody);
                    GUILayout.Space(4);
                    GUILayout.Label("If you truly cannot afford it, no hard feelings. Following us on X, sharing our work, or telling a friend helps more than you think.", new GUIStyle(_styleBody) { normal = { textColor = ColDim } });
                    GUILayout.Space(8);
                    GUILayout.BeginHorizontal();
                    GhostButton("Gumroad (regional pricing)", UrlGumroad, 32f);
                    GUILayout.Space(6);
                    GhostButton("Jinxxy (standard pricing)", UrlJinxxy, 32f);
                    GUILayout.EndHorizontal();
                });
                GUILayout.Space(10);
                EditorGUILayout.EndVertical();
            });
        }

        private void DrawDismiss()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(Pad);
            var style = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = ColText },
                hover = { textColor = ColText },
                active = { textColor = ColText }
            };
            Rect r = GUILayoutUtility.GetRect(10f, 28f, GUILayout.ExpandWidth(true));
            bool hover = r.Contains(Event.current.mousePosition);
            DrawRect(r, hover ? new Color(0.58f, 0.1f, 0.1f) : new Color(0.45f, 0.08f, 0.08f));
            if (hover) Repaint();
            GUI.Label(r, "I hate this popup, please make it go away", style);
            if (GUI.Button(r, GUIContent.none, GUIStyle.none))
            {
                EditorPrefs.SetBool(PrefDismissed, true);
                EditorUtility.DisplayDialog(
                    "Got it!",
                    "This popup will not open by itself anymore.\n\nIf you ever need it again:\nAntistar Assets > Show Popup Again",
                    "Ok, thanks!");
                Close();
            }
            GUILayout.Space(Pad);
            GUILayout.EndHorizontal();
        }

        private void DrawFooter()
        {
            GUILayout.Space(8);
            Separator();
            GUILayout.Space(6);
            GUILayout.Label("ANTISTAR · UGC, ASSET & GAMES STUDIO · Help window v2.0", _styleFooter);
            GUILayout.Space(10);
        }

        private void Section(string kicker, System.Action body)
        {
            GUILayout.Space(14);
            GUILayout.BeginHorizontal();
            GUILayout.Space(Pad);
            GUILayout.BeginVertical();
            GUILayout.Label(kicker, _styleKicker);
            GUILayout.Space(6);
            body();
            GUILayout.EndVertical();
            GUILayout.Space(Pad);
            GUILayout.EndHorizontal();
            GUILayout.Space(14);
            Separator();
        }

        private void Pane(System.Action body)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(Pad);
            GUILayout.BeginVertical();
            body();
            GUILayout.EndVertical();
            GUILayout.Space(Pad);
            GUILayout.EndHorizontal();
            GUILayout.Space(14);
        }

        private void Inset(System.Action body)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            GUILayout.BeginVertical();
            body();
            GUILayout.EndVertical();
            GUILayout.Space(12);
            GUILayout.EndHorizontal();
        }

        private void Faq(ref int index, string title, string body)
        {
            int i = index++;
            Rect header = GUILayoutUtility.GetRect(10f, 34f, GUILayout.ExpandWidth(true));
            DrawRect(header, ColSurface);
            if (header.Contains(Event.current.mousePosition))
            {
                DrawRect(header, ColSurface2);
                Repaint();
            }
            DrawRect(new Rect(header.x, header.y, 3f, header.height), _faqOpen[i] ? ColYellow : ColLine);

            var arrow = new GUIStyle(EditorStyles.label) { fontSize = 11, normal = { textColor = _faqOpen[i] ? ColYellow : ColDim } };
            GUI.Label(new Rect(header.x + 12, header.y + 8, 16, 20), _faqOpen[i] ? "▼" : "►", arrow);
            GUI.Label(new Rect(header.x + 32, header.y + 8, header.width - 40, 20), title, _styleFaqTitle);
            if (GUI.Button(header, GUIContent.none, GUIStyle.none)) _faqOpen[i] = !_faqOpen[i];

            if (_faqOpen[i])
            {
                Rect body2 = EditorGUILayout.BeginVertical();
                DrawRect(body2, ColSurface);
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label(body, _styleFaqBody);
                GUILayout.Space(8);
                GUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            GUILayout.Space(4);
        }

        private void StoreBox(string store, Color accent, Color background, string body)
        {
            Rect box = EditorGUILayout.BeginVertical();
            DrawRect(box, background);
            DrawRect(new Rect(box.x, box.y, 3f, box.height), accent);
            GUILayout.Space(8);
            Inset(() =>
            {
                var title = new GUIStyle(EditorStyles.boldLabel) { fontSize = 11, normal = { textColor = accent } };
                GUILayout.Label(store, title);
                GUILayout.Space(2);
                GUILayout.Label(body, _styleBody);
            });
            GUILayout.Space(8);
            EditorGUILayout.EndVertical();
        }

        private void Bullet(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(4);
            var mark = new GUIStyle(EditorStyles.label) { fontSize = 12, normal = { textColor = ColGreen } };
            GUILayout.Label("✓", mark, GUILayout.Width(16));
            GUILayout.Label(text, _styleBody);
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
        }

        private void PrimaryButton(string label, string url, float height)
        {
            Rect r = GUILayoutUtility.GetRect(10f, height, GUILayout.ExpandWidth(true));
            bool hover = r.Contains(Event.current.mousePosition);
            DrawRect(r, hover ? new Color(1f, 0.898f, 0.25f) : ColYellow);
            if (hover) Repaint();
            GUI.Label(r, label, _styleBtnPrimary);
            if (GUI.Button(r, GUIContent.none, GUIStyle.none))
                Application.OpenURL(url);
        }

        private void GhostButton(string label, string url, float height)
        {
            Color prev = GUI.backgroundColor;
            GUI.backgroundColor = ColSurface2;
            if (GUILayout.Button(label, _styleBtnGhost, GUILayout.Height(height)))
                Application.OpenURL(url);
            GUI.backgroundColor = prev;
        }

        private static void DrawRect(Rect rect, Color color)
        {
            if (Event.current.type != EventType.Repaint) return;
            EditorGUI.DrawRect(rect, color);
        }

        private void Separator()
        {
            Rect r = GUILayoutUtility.GetRect(position.width, 1f);
            DrawRect(r, ColLine);
        }
    }
}
#endif
