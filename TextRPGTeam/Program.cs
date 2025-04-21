namespace TextRPGTeam
{
    using System.Threading.Tasks.Dataflow;

    namespace TextRPG
    {
        internal class TextRPG
        {
            public enum ItemType { Weapon, Armor } // 아이템 타입 선언

            public enum DungeonDifficulty { Easy, Normal, Hard } // 던전 난이도 선언

            public class DungeonInfo
            {
                public int RecommendedDef { get; set; }
                public int BaseReward { get; set; }
                public DungeonInfo(int recommendedDef, int baseReward)
                {
                    RecommendedDef = recommendedDef;
                    BaseReward = baseReward;
                }
            }

            public class Player
            {
                public int Level { get; set; } = 1;
                public string Job { get; set; } = "전사";
                public int Attack { get; set; } = 10;
                public float Defence { get; set; } = 5f;
                public int Health { get; set; } = 100;
                public int MaxHealth { get; set; } = 100;
                public int Gold { get; set; } = 150000;
            }

            public class Item
            {
                // 아이템의 이름
                public string Name { get; set; }
                // 아이템의 장착 여부 (true: 장착됨, false: 비장착)
                public bool IsEquipped { get; set; }

                // 아이템의 판매 여부
                public bool isSold { get; set; }

                public ItemType Type { get; set; }

                //아이템의 스테이터스 보너스
                public int AttackBonus { get; set; }
                public float DefenceBonus { get; set; }
                public int MaxHealthBonus { get; set; }

                // 아이템의 가격
                public int Price { get; set; }

                // 부가 설명 (아이템 효과나 배경 스토리 등)
                public string Description { get; set; }

                // 생성자
                public Item(string name, ItemType type, int attackBonus, float defenceBonus, int maxHealthBonus, string description, int price)
                {
                    Name = name;
                    Type = type;
                    AttackBonus = attackBonus;
                    DefenceBonus = defenceBonus;
                    MaxHealthBonus = maxHealthBonus;
                    Description = description;
                    Price = price;
                    IsEquipped = false;
                    isSold = false;
                }
            }

            public class Shop
            {
                private List<Item> items;

                public Shop()
                {
                    items = new List<Item>();
                }

                public void AddItem(Item item)
                {
                    items.Add(item);
                }

                public void RemoveItem(Item item)
                {
                    items.Remove(item);
                }

                public void PrintShop()
                {
                    foreach (var item in items)
                    {
                        // isSold가 true이면 "구매 완료", 아니라면 가격을 출력
                        string status = item.isSold ? "구매 완료" : $"{item.Price} G";
                        // 아이템 이름, 설명 및 상태를 출력 (원하는 대로 구분자를 추가)
                        Console.WriteLine($"{item.Name}{item.Description} || {status}");
                    }
                }

                public List<Item> GetItems()
                {
                    return items;
                }
            }

            public class Inventory
            {
                // 인벤토리 내의 아이템들을 저장할 리스트
                private List<Item> items;

                // 생성자: 인벤토리 생성 시 리스트를 초기화
                public Inventory()
                {
                    items = new List<Item>();
                }

                // 아이템 추가 메서드
                public void AddItem(Item item)
                {
                    items.Add(item);
                }

                // 아이템 제거 메서드
                public void RemoveItem(Item item)
                {
                    items.Remove(item);
                }

                // 아이템 장착 메서드
                public void EquipItem(Item item, Player player)
                {
                    item.IsEquipped = true;
                    player.Attack += item.AttackBonus;
                    player.Defence += item.DefenceBonus;
                    player.MaxHealth += item.MaxHealthBonus;
                }

                // 아이템 장착 해제 메서드
                public void UnequipItem(Item item, Player player)
                {
                    item.IsEquipped = false;
                    player.Attack -= item.AttackBonus;
                    player.Defence -= item.DefenceBonus;
                    player.MaxHealth -= item.MaxHealthBonus;
                    player.Health = Math.Min(player.Health, player.MaxHealth);
                }

                // 현재 인벤토리 상태를 출력하는 메서드
                public void PrintInventory()
                {
                    Console.WriteLine("[아이템 목록]");
                    foreach (var item in items)
                    {
                        // 각 아이템의 이름과 장착 상태를 출력합니다.
                        Console.WriteLine($"{(item.IsEquipped ? "[E]" : "")} {item.Name}{item.Description}");
                    }
                }

                // 인벤토리 내 아이템 목록을 반환하는 메서드 (장착관리 화면에서 사용)

                public List<Item> GetItems()
                {
                    return items;
                }
            }

            public static void Main(string[] args)
            {
                Console.WriteLine("어서오세요! 텍스트 RPG의 세계에 온 것을 환영합니다!");
                Console.WriteLine("모험을 시작할 주인공의 이름을 정해주세요!");
                string nickname = Console.ReadLine().Trim();
                Console.WriteLine($"반갑습니다! {nickname}님! 모험을 시작해볼까요?");

                Inventory myInventory = new Inventory(); // 인벤토리 생성
                Player player = new Player(); // 플레이어 생성
                Shop shop = new Shop(); // 상점 생성

                shop.AddItem(new Item("수련자 갑옷", ItemType.Armor, 0, 5, 0, " || 방어구 || 방어력 +5 || 수련에 도움을 주는 갑옷입니다.", 1000));
                shop.AddItem(new Item("무쇠 갑옷", ItemType.Armor, 0, 9, 0, " || 방어구 || 방어력 +9 || 스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 2000));
                shop.AddItem(new Item("스파르타의 갑옷", ItemType.Armor, 0, 15, 0, " || 방어구|| 방어력 +15 || 수련에 도움을 주는 갑옷입니다. ", 3500));
                shop.AddItem(new Item("불타는 지옥의 갑주", ItemType.Armor, 0, 50, 300, " || 방어구 || 방어력 +50, 체력 +300 || 지옥의 기사가 입고있던 불타는 갑주입니다. 화상주의!", 10500));
                shop.AddItem(new Item("낡은 검", ItemType.Weapon, 2, 0, 0, " || 무기 || 공격력 +2 || 쉽게 볼 수 있는 낡은 검 입니다.", 600));
                shop.AddItem(new Item("청동 도끼", ItemType.Weapon, 5, 0, 0, " || 무기 || 공격력 +5 || 어디선가 사용됐던거 같은 도끼입니다.", 1500));
                shop.AddItem(new Item("스파르타의 창", ItemType.Weapon, 7, 0, 0, " || 무기 || 공격력 +7 || 스파르타의 전사들이 사용했다는 전설의 창입니다.", 2500));
                shop.AddItem(new Item("빛나는 천상의 양손검", ItemType.Weapon, 30, 5, 100, " || 무기 || 공격력 +30, 방어력 +5, 체력 +100 || 천상의 수호자가 지녔던 양손검입니다. 오래 쳐다보면 눈이 아파요!", 8000));

                Console.ReadLine();

                ViewVillage();

                int CheckChoice(int min, int max, Action printMenu)
                {
                    printMenu();                     // 메뉴 한 번만 출력
                    while (true)
                    {
                        Console.Write(">> ");
                        string input = Console.ReadLine().Trim();

                        if (!int.TryParse(input, out int choice))
                        {
                            // 잘못된 입력: 화면 클리어 + 메뉴+오류 메시지
                            Console.Clear();
                            printMenu();
                            Console.WriteLine("숫자로 입력해주세요!");
                            continue;
                        }

                        if (choice < min || choice > max)
                        {
                            Console.Clear();
                            printMenu();
                            Console.WriteLine($"{min}부터 {max} 사이의 숫자를 입력해주세요!");
                            continue;
                        }

                        return choice;
                    }
                }

                void ViewVillage()
                {
                    int c = CheckChoice(1, 5, () =>
                    {
                        Console.Clear();
                        Console.WriteLine("이곳은 마을입니다!");
                        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                        Console.WriteLine();
                        Console.WriteLine("1. 스테이터스   2. 인벤토리   3. 상점   4. 던전입장   5. 휴식");
                    });

                    switch (c)
                    {
                        case 1: ViewStatus(player); break;
                        case 2: ViewInventory(); break;
                        case 3: ViewShop(); break;
                        case 4: EnterDungeon(); break;
                        case 5: TakeRest(); break;
                    }
                }

                void TakeRest()
                {
                    int c = CheckChoice(0, 1, () =>
                    {
                        Console.Clear();
                        Console.WriteLine("< 휴식하기 >");
                        Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G, 현재 {player.Health}/{player.MaxHealth})");
                        Console.WriteLine();
                        Console.WriteLine("1. 휴식하기   0. 나가기");
                    });

                    switch (c)
                    {
                        case 1:
                            {
                                if (player.Gold >= 500)
                                {
                                    player.Gold -= 500;
                                    player.Health = Math.Min(player.Health + 100, player.MaxHealth);

                                    Console.WriteLine($"체력을 100 회복합니다. (현재 {player.Health}/{player.MaxHealth})");
                                    Console.WriteLine("엔터키를 누르면 마을로 돌아갑니다!");
                                    Console.ReadLine();
                                    ViewVillage();
                                }
                                else
                                {
                                    Console.WriteLine("보유 Gold가 부족합니다!");
                                    TakeRest();
                                }
                            }
                            break;
                        case 0: ViewVillage(); break;
                    }
                }


                void EnterDungeon()
                {
                    int c = CheckChoice(0, 3, () =>
                    {
                        Console.Clear();
                        Console.WriteLine("< 던전 입장 >");
                        Console.WriteLine("다양한 난이도의 던전이 준비되어 있습니다.");
                        Console.WriteLine("권장하는 능력치보다 낮을 경우, 40% 확률로 클리어를 실패할 수 있습니다.");
                        Console.WriteLine();
                        Console.WriteLine(" 1. 쉬운 던전   | 방어력 5 이상 권장");
                        Console.WriteLine(" 2. 일반 던전   | 방어력 11 이상 권장");
                        Console.WriteLine(" 3. 어려운 던전   | 방어력 17 이상 권장");
                        Console.WriteLine(" 0. 나가기");
                        Console.WriteLine();
                        Console.Write("원하시는 행동을 입력해주세요.");
                        Console.WriteLine();
                    });

                    switch (c)
                    {
                        case 0:
                            ViewVillage();
                            return;

                        case 1:
                            RunDungeon(DungeonDifficulty.Easy);
                            break;
                        case 2:
                            RunDungeon(DungeonDifficulty.Normal);
                            break;
                        case 3:
                            RunDungeon(DungeonDifficulty.Hard);
                            break;
                    }
                }

                void RunDungeon(DungeonDifficulty difficulty)
                {
                    // 난이도별 정보
                    var info = difficulty switch
                    {
                        DungeonDifficulty.Easy => new DungeonInfo(5, 1000),
                        DungeonDifficulty.Normal => new DungeonInfo(11, 1700),
                        DungeonDifficulty.Hard => new DungeonInfo(17, 2500),
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    var rnd = new Random();
                    int recDef = info.RecommendedDef;
                    int baseG = info.BaseReward;

                    // 실패 판정: 방어력 부족 시 40% 실패
                    bool failed = (player.Defence < recDef) && (rnd.NextDouble() < 0.4);

                    // 데미지 범위 계산
                    float delta = player.Defence - recDef;
                    float minDmg = Math.Max(20 - delta, 1);
                    float maxDmg = Math.Max(35 - delta, minDmg);
                    int damage = rnd.Next((int)Math.Floor(minDmg), (int)Math.Ceiling(maxDmg) + 1);
                    if (failed) damage /= 2;
                    player.Health = Math.Max(player.Health - damage, 0);

                    // 결과 출력
                    if (player.Health == 0)
                    {
                        Console.WriteLine("당신은 사망했습니다..");
                        Console.WriteLine("체력이 0이 되어 마을로 강제 귀환합니다!");
                        Console.WriteLine("가지고 있던 골드의 절반을 잃었습니다..");

                        player.Health = 1;
                        int lostGold = player.Gold / 2;

                        Console.WriteLine($"현재 체력: {player.Health}/{player.MaxHealth}");
                        Console.WriteLine($"현재 골드: {player.Gold - lostGold} (-{lostGold})");

                        player.Gold -= lostGold;

                        Console.WriteLine("엔터키를 누르면 마을로 돌아갑니다.");
                        Console.ReadLine();
                        ViewVillage();
                        return;  // 던전 후속 처리(보상 등)를 건너뛰고 바로 돌아가기
                    }

                    if (failed)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"던전 실패! 체력 -{damage}  (현재 {player.Health}/{player.MaxHealth})");
                    }
                    else
                    {
                        // 추가 보상 비율
                        int pct = rnd.Next(player.Attack, player.Attack * 2 + 1);
                        int bonusGold = baseG * pct / 100;
                        int totalReward = baseG + bonusGold;
                        player.Gold += totalReward;

                        Console.WriteLine();
                        Console.WriteLine($"던전 클리어! 체력 -{damage}  (현재 {player.Health}/{player.MaxHealth})");
                        Console.WriteLine($"보상: 기본 {baseG}G + 추가 {pct}%({bonusGold}G) = {totalReward}G");
                    }

                    Console.WriteLine("엔터키를 누르면 마을로 돌아갑니다.");
                    Console.ReadLine();
                    ViewVillage();
                }

                void ViewStatus(Player p)
                {
                    var equipped = myInventory.GetItems().Where(i => i.IsEquipped);

                    int atkBonus = equipped.Sum(i => i.AttackBonus);
                    float defBonus = equipped.Sum(i => i.DefenceBonus);
                    int hpBonus = equipped.Sum(i => i.MaxHealthBonus);

                    int c = CheckChoice(0, 0, () =>
                    {
                        Console.Clear();
                        Console.WriteLine("< 스테이터스 >");

                        var atkBuff = atkBonus > 0 ? $" (+{atkBonus})" : "";
                        var defBuff = defBonus > 0 ? $" (+{defBonus})" : "";
                        var hpBuff = hpBonus > 0 ? $" (+{hpBonus})" : "";

                        Console.WriteLine($" Lv.{p.Level} \n 직업 : {p.Job} \n 공격 : {p.Attack}{atkBuff} \n 방어 : {p.Defence}{defBuff} \n 체력 : {p.Health} / {p.MaxHealth}{hpBuff} \n 골드 : {p.Gold} G");
                        Console.WriteLine();
                        Console.WriteLine("0. 나가기");
                    });
                    if (c == 0) ViewVillage();
                }

                void ViewInventory()
                {
                    int c = CheckChoice(0, 1, () =>
                    {
                        Console.Clear();
                        Console.WriteLine("< 인벤토리 >");
                        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                        myInventory.PrintInventory();
                        Console.WriteLine();
                        Console.WriteLine("1. 장착 관리   0. 나가기");
                    });

                    if (c == 0) ViewVillage();
                    else ViewEquipment();
                }

                void ViewEquipment()
                {
                    var list = myInventory.GetItems();
                    int c = CheckChoice(0, list.Count, () =>
                    {
                        Console.Clear();
                        Console.WriteLine("< 인벤토리 - 장착 관리 >");
                        Console.WriteLine("같은 종류의 아이템은 하나만 장착할 수 있습니다.");
                        Console.WriteLine("[아이템 목록]");
                        for (int i = 0; i < list.Count; i++)
                            Console.WriteLine($"{i + 1}.{(list[i].IsEquipped ? "[E] " : "")} {list[i].Name}{list[i].Description}");
                        Console.WriteLine();
                        Console.WriteLine("0. 나가기");
                    });

                    if (c == 0) { ViewInventory(); return; }

                    var it = list[c - 1];
                    if (it.IsEquipped)
                    {
                        myInventory.UnequipItem(it, player);
                    }
                    else
                    {
                        var currentlyEquipped = list.Find(item => item.IsEquipped && item.Type == it.Type);
                        if (currentlyEquipped != null)
                        {
                            myInventory.UnequipItem(currentlyEquipped, player);
                        }
                        // 2) 새 장비 장착
                        myInventory.EquipItem(it, player);
                    }

                    // 장착 후 인벤토리로 복귀
                    ViewInventory();
                }

                void ViewShop()
                {
                    int c = CheckChoice(0, 2, () =>
                    {
                        Console.Clear();
                        Console.WriteLine("< 상점 >");
                        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                        Console.WriteLine();
                        Console.WriteLine("[보유 골드]");
                        Console.WriteLine($"{player.Gold} G");
                        Console.WriteLine();
                        shop.PrintShop();   // “이름 – 설명 – {가격 or 구매 완료}”
                        Console.WriteLine();
                        Console.WriteLine("1. 구매   2. 판매   0. 나가기");
                    });

                    switch (c)
                    {
                        case 1: PurchaseItem(); break;
                        case 2: SellItem(); break;
                        case 0: ViewVillage(); break;
                    }
                }

                void PurchaseItem()
                {
                    var list = shop.GetItems();
                    int c = CheckChoice(0, list.Count, () =>
                    {
                        Console.Clear();
                        Console.WriteLine("< 상점 - 아이템 구매 >");
                        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                        Console.WriteLine();
                        Console.WriteLine("[보유 골드]");
                        Console.WriteLine($"{player.Gold} G");
                        Console.WriteLine();
                        for (int i = 0; i < list.Count; i++)
                        {
                            var it = list[i];
                            var status = it.isSold ? "구매 완료" : $"{it.Price}G";
                            Console.WriteLine($"{i + 1}. {it.Name}{it.Description} || {status}");
                        }
                        Console.WriteLine();
                        Console.WriteLine("0. 나가기");
                    });

                    if (c == 0) { ViewShop(); return; }

                    var sel = list[c - 1];
                    if (sel.isSold)
                        Console.WriteLine("이미 구매한 아이템입니다.");
                    else if (player.Gold >= sel.Price)
                    {
                        player.Gold -= sel.Price;
                        sel.isSold = true;
                        myInventory.AddItem(sel);
                        Console.WriteLine("구매 완료!");
                    }
                    else
                        Console.WriteLine("Gold가 부족합니다.");

                    Console.WriteLine("엔터키를 누르면 상점으로 돌아갑니다.");
                    Console.ReadLine();
                    ViewShop();
                }
                void SellItem()
                {
                    var list = myInventory.GetItems();
                    int c = CheckChoice(0, list.Count, () =>
                    {
                        Console.Clear();
                        Console.WriteLine("< 상점 - 아이템 판매 >");
                        Console.WriteLine("가지고 있는 아이템을 팔 수 있는 상점입니다.");
                        Console.WriteLine();
                        Console.WriteLine("[보유 골드]");
                        Console.WriteLine($"{player.Gold} G");
                        Console.WriteLine();
                        for (int i = 0; i < list.Count; i++)
                        {
                            var it = list[i];
                            var sellPrice = (it.Price * 85) / 100;
                            Console.WriteLine($"{i + 1}. {it.Name}{it.Description} || {sellPrice} G");
                        }
                        Console.WriteLine();
                        Console.WriteLine("0. 나가기");
                    });

                    if (c == 0) { ViewShop(); return; }

                    var sel = list[c - 1];

                    player.Gold += (sel.Price * 85) / 100;
                    sel.isSold = false;
                    if (sel.IsEquipped)
                    {
                        myInventory.UnequipItem(sel, player);
                        player.Health = Math.Min(player.Health, player.MaxHealth);
                    }
                    myInventory.RemoveItem(sel);
                    Console.WriteLine("판매 완료!");

                    Console.WriteLine("엔터키를 누르면 상점으로 돌아갑니다.");
                    Console.ReadLine();
                    ViewShop();
                }
            }
        }
    }
}