"use strict";

define(["umbra", "umbra.instance"], function (u, umbraInstance) {


    u.umbra.addViewType(
        new u.ViewType(
            "BurnSystems.FlsxBG.DetailView.VoxelMapView",
            function (info) {
                info.viewPoint.domContent.html(
                    "Jo");
            }));
});