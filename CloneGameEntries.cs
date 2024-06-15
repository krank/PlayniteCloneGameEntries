using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CloneGameEntries
{
  public class CloneGameEntries : GenericPlugin
  {
    private static readonly ILogger logger = LogManager.GetLogger();

    private CloneGameEntriesSettingsViewModel settings { get; set; }

    public override Guid Id { get; } = Guid.Parse("42552228-8e75-451c-8b3b-bb20fe4f168b");

    public CloneGameEntries(IPlayniteAPI api) : base(api)
    {
      settings = new CloneGameEntriesSettingsViewModel(this);
      Properties = new GenericPluginProperties
      {
        HasSettings = true
      };
    }

    public override IEnumerable<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
    {
      yield return new GameMenuItem
      {
        Description = $"Make clone of {args.Games.Count} entries",
        Action = (gameArgs) =>
        {
          foreach (Game game in args.Games)
          {
            Game copy = CloneGame(game);
            PlayniteApi.Database.Games.Add(copy);
          }
        }
      };
    }

    private Game CloneGame(Game original)
    {
      string cloneString = ResourceProvider.GetString("LOCCloneGameEntryCloneTitle");

      Game copy = new Game()
      {
        // MAIN META
        Name = $"{original.Name} [{cloneString}]",
        SortingName = original.SortingName,
        Description = original.Description,
        Version = original.Version,

        Manual = original.Manual,
        ReleaseDate = ReleaseDate.Deserialize(original.ReleaseDate.Value.Serialize()),
        RegionIds = CopyOrNull(original.RegionIds),
        Links = CopyOrNull(original.Links),

        TagIds = CopyOrNull(original.TagIds),
        PublisherIds = CopyOrNull(original.PublisherIds),
        DeveloperIds = CopyOrNull(original.DeveloperIds),
        SeriesIds = CopyOrNull(original.SeriesIds),
        GenreIds = CopyOrNull(original.GenreIds),
        CategoryIds = CopyOrNull(original.AgeRatingIds),
        FeatureIds = CopyOrNull(original.FeatureIds),

        AgeRatingIds = CopyOrNull(original.AgeRatingIds),
        CompletionStatusId = original.CompletionStatusId,

        // USER-PROVIDED METADATA
        Notes = original.Notes,
        Favorite = original.Favorite,
        Hidden = original.Hidden,
        UserScore = original.UserScore,

        // GAME DATA
        SourceId = original.SourceId,
        GameId = original.GameId,
        PlatformIds = CopyOrNull(original.PlatformIds),
        InstallDirectory = original.InstallDirectory,
        Roms = CopyOrNull(original.Roms),
        GameActions = CopyOrNull(original.GameActions),

        PluginId = original.PluginId,
        IncludeLibraryPluginAction = original.IncludeLibraryPluginAction,

        // SCORES
        CommunityScore = original.CommunityScore,
        CriticScore = original.CriticScore,

        // IMAGES
        Icon = original.Icon,
        BackgroundImage = original.BackgroundImage,
        CoverImage = original.CoverImage,

        // SCRIPTS
        GameStartedScript = original.GameStartedScript,
        PostScript = original.PostScript,
        PreScript = original.PreScript,
        UseGlobalGameStartedScript = original.UseGlobalGameStartedScript,
        UseGlobalPostScript = original.UseGlobalPostScript,
        UseGlobalPreScript = original.UseGlobalPreScript,

        // GAME STATUS
        IsInstalled = original.IsInstalled,
        IsInstalling = original.IsInstalling,
        IsLaunching = original.IsLaunching,
        IsRunning = original.IsRunning,
        IsUninstalling = original.IsUninstalling,
      };

      return copy;
    }

    public List<T> CopyOrNull<T>(List<T> original)
    {
      return original != null ? new List<T>(original) : null;
    }

    public ObservableCollection<T> CopyOrNull<T>(ObservableCollection<T> original)
    {
      return original != null ? new ObservableCollection<T>(original) : null;
    }


    public override ISettings GetSettings(bool firstRunSettings)
    {
      return settings;
    }

    public override UserControl GetSettingsView(bool firstRunSettings)
    {
      return new CloneGameEntriesSettingsView();
    }
  }
}