namespace DspicoThemeForms.Core.Runners;

public sealed class PtexConvResult
{
    public bool HasRunTopBackground { get; set; }
    public bool HasRunBottomBackground { get; set; }
    public bool HasRunGridCell { get; set; }
    public bool HasRunBannerListCell { get; set; }
    public bool HasRunGridCellSelected { get; set; }
    public bool HasRunScrim { get; set; }
    public bool HasRunBannerListCellSelected { get; set; }

    public bool NoneRan => !HasRunTopBackground && !HasRunBottomBackground && !HasRunGridCell && !HasRunBannerListCell && !HasRunGridCellSelected && !HasRunScrim && !HasRunBannerListCellSelected;

    public bool SomeRan => HasRunTopBackground || HasRunBottomBackground || HasRunGridCell || HasRunBannerListCell || HasRunGridCellSelected || HasRunScrim || HasRunBannerListCellSelected;

    public bool AllRan => HasRunTopBackground && HasRunBottomBackground && HasRunGridCell && HasRunBannerListCell && HasRunGridCellSelected && HasRunScrim && HasRunBannerListCellSelected;
}

